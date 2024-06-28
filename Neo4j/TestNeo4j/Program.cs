using Neo4j.Driver;
using System.Data;
using System.Security.Cryptography;

namespace TestNeo4j
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            DataTable? dataTable = CSVUtil.FromCSV("D:\\admin\\Desktop\\neo4jTestData\\relations.csv");
            if (dataTable == null)
            {
                Console.WriteLine("读取CSV失败！");
                return;
            }
            // source_entity_id,target_entity_id,relation_type
            // 连接设置
            var uri = "bolt://localhost:7687";
            var user = "neo4j";
            var password = "admin@123"; 

            var driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
            var session = driver.AsyncSession();
            try
            {
                foreach (var row in dataTable.Rows.Cast<DataRow>())
                {
                    object tid_val = row["target_entity_id"];
                    object sid_val = row["source_entity_id"];
                    object rname_val = row["relation_type"];
                    if (sid_val != null)
                    {
                        await CreateNode(session, sid_val.ToString());
                    }
                    if (tid_val != null)
                    {
                        await CreateNode(session, tid_val.ToString());
                    }
                    if (sid_val != null && tid_val != null && rname_val != null)
                    {
                        await CreateRelation(session, sid_val.ToString(), tid_val.ToString(), rname_val.ToString());
                    }
                }

                //// 读取节点
                //var readResult = await session.RunAsync("MATCH (n:Person) WHERE n.name = $name RETURN n",
                //    new { name = "Alice" });

                //var readNode = await readResult.SingleAsync();
                //Console.WriteLine($"Read Node: {readNode["n"].As<INode>().Properties["name"]}");

                //// 更新节点
                //var updateResult = await session.RunAsync("MATCH (n:Person) WHERE n.name = $name SET n.age = $age RETURN n",
                //    new { name = "Alice", age = 35 });

                //var updatedNode = await updateResult.SingleAsync();
                //Console.WriteLine($"Updated Node Age: {updatedNode["n"].As<INode>().Properties["age"]}");

                //// 删除节点
                //var deleteResult = await session.RunAsync("MATCH (n:Person) WHERE n.name = $name DELETE n",
                //    new { name = "Alice" });

                //Console.WriteLine("Deleted Node: Alice");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                await session.CloseAsync();
                await driver.CloseAsync();
            }
        }

        static async Task CreateNode(IAsyncSession session, string id)
        {
            // 使用 MERGE 创建节点，如果节点已存在则不会重复创建
            var mergeNode = await session.RunAsync(@"
                    MERGE (n:GeoEntity {id: $id})
                    RETURN n",
                new { id });

            var node = await mergeNode.SingleAsync();
            Console.WriteLine($"Created Node: {node["n"].As<INode>().Properties["id"]}");
        }
        static async Task CreateRelation(IAsyncSession session, string sid, string tid, string rname)
        {
            // 创建关系 KNOWS
            var createRelationship = await session.RunAsync(@"
                    MATCH (a:GeoEntity {id: $sid}), (b:GeoEntity {id: $tid})
                    CREATE (a)-[r:GeoEntityRelation {name: $rname}]->(b)
                    RETURN r",
                new { sid, tid, rname });
            var relationship = await createRelationship.SingleAsync();
            Console.WriteLine($"Created Relation: {sid} {relationship["r"].As<IRelationship>().Properties["name"]} {tid}");
        }
    }
}
