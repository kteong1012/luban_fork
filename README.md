# 手动添加LazyLoad模块到自己工程的步骤

commit地址： https://github.com/kteong1012/luban_fork/commit/32b9077e449e090e7f0330e3ef32fe09898bb193

### 1. 克隆这个工程的lazy_load分支。
### 2. 手动复制新增文件
![1713412013696](https://github.com/kteong1012/luban_fork/assets/44496710/09bb3f6d-b5e6-4cad-80be-954400ef0afb)
### 3. 修改csproj文件，增加模板
![1713412118598](https://github.com/kteong1012/luban_fork/assets/44496710/79164b41-5173-4c39-a659-d2dda4a2c165)
### 4. 编译Luban工程，升成新的Luban.dll并用它替换旧的
### 5. 命令行
![1713412120951](https://github.com/kteong1012/luban_fork/assets/44496710/966d602f-c4b1-4285-bf96-6a3ab053b60e)
### 6. 运行时代码在官方库的examples里有，可供参考。
路径： https://github.com/focus-creative-games/luban_examples/tree/main/Projects/Csharp_Unity_LazyLoad_bin
![image](https://github.com/kteong1012/luban_fork/assets/44496710/3736804b-b51f-43f9-a518-091c69cd5f21)
