// OsgReader.cpp : 定义静态库的函数。
//

#include "pch.h"
#include "framework.h"
#include <osg/Node>
#include <osg/Geode>
#include <osg/ShapeDrawable>
#include <osg/BoundingBox>
#include <osgViewer/Viewer>
#include <osgDB/ReadFile>
#include <iostream>

// 计算节点的边界框
osg::BoundingBox getBoundingBox(osg::ref_ptr<osg::Node> node)
{
    osg::BoundingBox bbox;

    // 检查节点是否为空
    if (node)
    {
        // 如果节点是几何体（Geode），则访问其图形数据
        osg::ref_ptr<osg::Geode> geode = dynamic_cast<osg::Geode*>(node.get());
        if (geode)
        {
            for (unsigned int i = 0; i < geode->getNumDrawables(); ++i)
            {
                // 获取每个几何体的边界框
                osg::BoundingBox drawable_bbox = geode->getDrawable(i)->getBoundingBox();
                bbox.expandBy(drawable_bbox);  // 扩展总边界框
            }
        }
        else
        {
            // 如果是其他类型的节点，递归获取边界框
            node->accept([&bbox](osg::Node& child) {
                bbox.expandBy(child.getBoundingBox());
                });
        }
    }
    return bbox;
}

int main(int argc, char** argv)
{
    // 加载模型文件（假设模型文件是一个 .obj 文件）
    osg::ref_ptr<osg::Node> rootNode = osgDB::readNodeFile("model.obj");

    if (!rootNode)
    {
        std::cerr << "Unable to load model file!" << std::endl;
        return 1;
    }

    // 获取模型的边界框
    osg::BoundingBox bbox = getBoundingBox(rootNode);

    // 输出边界框的最小和最大坐标
    std::cout << "Bounding Box Min: ("
        << bbox._min.x() << ", " << bbox._min.y() << ", " << bbox._min.z() << ")" << std::endl;
    std::cout << "Bounding Box Max: ("
        << bbox._max.x() << ", " << bbox._max.y() << ", " << bbox._max.z() << ")" << std::endl;

    // 创建一个简单的查看器
    osgViewer::Viewer viewer;
    viewer.setSceneData(rootNode);
    return viewer.run();
}
