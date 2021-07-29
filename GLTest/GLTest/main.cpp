#include<glad/glad.h>
#include<GLFW/glfw3.h>
#include<iostream>

using namespace std;

void framebuffer_size_callback(GLFWwindow* window, int width, int height);
void processInput(GLFWwindow* window);

int main() 
{
	glfwInit();//初始化glfw
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);//设置版本号为3
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);//核心模式(Core-profile)

	GLFWwindow* window = glfwCreateWindow(800, 600, "MyOpenGLWindow", NULL, NULL);
	if (window == NULL)
	{
		cout << "Failed to create GLFW window" << endl;
		glfwTerminate();

		return -1;
	}
	glfwMakeContextCurrent(window);

	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))//初始化GLAD
	{
		cout << "Failed to init GLAD" << endl;

		return -1;
	}

	//glViewport(0, 0, 800, 600);//视口 前两个左下角位置 后两个宽高
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);//每当窗口改变大小，GLFW会调用这个函数

	while (!glfwWindowShouldClose(window))
	{
		processInput(window);

		glfwSwapBuffers(window);//检测触发事件
		glfwPollEvents();//交换颜色缓冲（双缓冲）
	}

	glfwTerminate();//释放资源

	return 0;
}

void framebuffer_size_callback(GLFWwindow* window, int width, int height) 
{
	glViewport(0, 0, width, height);
}

void processInput(GLFWwindow* window)
{
	if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS) {
		glfwSetWindowShouldClose(window, true);
	}
}