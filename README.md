### 使用CSharp裁剪字体

- 1 从[IKVM官网](http://www.ikvm.net/download.html)下[https://sourceforge.net/projects/ikvm/files/](https://sourceforge.net/projects/ikvm/files/)下载编译好的组件

- 2 在bin文件夹下启动cmd, 输入ikvmc会出现相应的help

- 3 搜索出来的转换jar方法：

    常用参数说明
    -target:library 
    使用方法： ikvmc -target:library a.jar 
    由于我们的目的是把jar转为dll,此参数就是此作用

    -reference:<filespec>(-r:<filespec>) 
    使用方法: ikvmc -target:library a.jar -r:b.dll 
    该方法作用若a.jar存在第三方的引用,则我们需要指明需要引用的dll

- 4 生成编辑的命令行文件
```
GFontClipper 1.0.0
Copyright (C) 2019 GFontClipper

  -f, --file      Required. 转换字体文件

  -s, --src       Required. 源字体地址

  -t, --target    Required. 转换成的地址文件

  --help          Display this help screen.

  --version       Display version information.
```

-f 使用的文件可自行从文件中抽取