#include<glad/glad.h>
#include<GLFW/glfw3.h>
#include<iostream>

using namespace std;

void framebuffer_size_callback(GLFWwindow* window, int width, int height);
void processInput(GLFWwindow* window);

int main() 
{
	glfwInit();//��ʼ��glfw
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);//���ð汾��Ϊ3
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);//����ģʽ(Core-profile)

	GLFWwindow* window = glfwCreateWindow(800, 600, "MyOpenGLWindow", NULL, NULL);
	if (window == NULL)
	{
		cout << "Failed to create GLFW window" << endl;
		glfwTerminate();

		return -1;
	}
	glfwMakeContextCurrent(window);

	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))//��ʼ��GLAD
	{
		cout << "Failed to init GLAD" << endl;

		return -1;
	}

	//glViewport(0, 0, 800, 600);//�ӿ� ǰ�������½�λ�� ���������
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);//ÿ�����ڸı��С��GLFW������������

	while (!glfwWindowShouldClose(window))
	{
		processInput(window);

		glfwSwapBuffers(window);//��ⴥ���¼�
		glfwPollEvents();//������ɫ���壨˫���壩
	}

	glfwTerminate();//�ͷ���Դ

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