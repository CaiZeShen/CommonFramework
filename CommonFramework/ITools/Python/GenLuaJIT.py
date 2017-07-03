#coding=utf-8
import os
import sys
import shutil  

def ExcuseCommand(arg):
	if  os.system(arg) != 0:
		print(u"执行成功 = " + arg);
	else:
		print(u"执行失败 = " + arg);


def GetFileList(dir):
	dir = str(dir);
	if dir == "":
		return []

	dir = dir.replace("/","\\");
	files = os.listdir(dir);

	# 把当前的文件夹遍历一般
	results = [x for  x in files if os.path.isfile(dir + x)]

	return results;


# 删除指定目录
def RemovePath(rootPath):
	if os.path.exists(rootPath):
		fileList = os.listdir(rootPath)
		for file in fileList:
			filePath = os.path.join(rootPath,file)
			if os.path.isfile(filePath):
				os.remove(filePath)
			elif os.path.isdir(filePath):
				if len(filePath) > 2:
					shutil.rmtree(filePath,True)


# 加密编译
def  ComplieLua(filePath,file,frontPath):
	# 输出路径
	outPath = frontPath + "\\out" + file[len(frontPath):]
	#相对路径
	nPos = len(filePath) -len(frontPath)
	delaPath = frontPath + "\\out" + filePath[len(frontPath):len(frontPath) + nPos]

	if  not os.path.exists(delaPath):
		os.makedirs(delaPath)

	curPath = os.getcwd()

	os.chdir(luaJITPath)

	commond = "luajit -b %s %s"%(file,outPath)

	ExcuseCommand(commond)

	os.chdir(curPath)


def  CopyFile(src,dst,symLinks = False,ignore = None):
	for item in os.listdir(src):
		# 源目录
		s = os.path.join(src,item)
		# 目标目录
		d = os.path.join(dst,item)

		if os.path.isdir(s):
			if not os.path.exists(d):
				os.makedirs(d)
			CopyFile(s,d,symLinks,ignore)
		elif os.path.isfile(s):
			if not os.path.exists(d):
				os.makedirs(d)
			shutil.copy2(s,d)


# 把当前的lua遍历出来
def ListFile(src,frontPath):
	for item in os.listdir(src):
		s = os.path.join(src,item)

		if os.path.isdir(s):
			ListFile(s,frontPath)
		elif s.endswith(".lua"):
			ComplieLua(src,s,frontPath)





curPath = os.getcwd() + "\\";

# 参数为true表示在文件夹内点击启动
if  len(sys.argv) > 1 and sys.argv[1] == "True":	
	nPos = curPath.index("ITools");
	curPath = curPath[:nPos];

print("cureent path = " + curPath);

luaPath = curPath + "Assets\\Lua"
toLuaPath = curPath + "Assets\\ToLua\\Lua"
winStreamAssetPath = curPath + "Assets\\StreamingAssets\\Lua\\Windows"
luaJITPath = curPath + "ITools\\LuaJIT"

def Main():
	RemovePath(winStreamAssetPath);

	# 切换路径
	os.chdir(luaPath)

	# 临时存放路径
	outPath = luaPath + "\\out"

	if  os.path.exists(outPath):
		RemovePath(outPath)
	else:
		os.makedir(outPath)

	ListFile(luaPath,luaPath)

	CopyFile(outPath,winStreamAssetPath)

	RemovePath(outPath)

Main()