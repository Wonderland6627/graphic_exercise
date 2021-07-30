#include<glad/glad.h>
#include<GLFW/glfw3.h>
#include<iostream>

#include"shader.h"

using namespace std;

const unsigned int SCR_WIDTH = 800;
const unsigned int SCR_HEIGHT = 800;

const char* vertexShaderSource =	"#version 330 core\n"//顶点着色器
									"layout(location = 0) in vec3 aPos;\n"//位置变量的属性位置值为0
									"layout(location = 1) in vec3 aColor;\n"//颜色变量的属性位置值为1
								    "out vec3 vertexColor;\n"//向片段着色器输出一个颜色 接收要同名同类型
									"void main()\n"
									"{\n"
									"	gl_Position = vec4(aPos, 1.0);\n"
									"	vertexColor = aColor;\n"//将vertexColor设置为从顶点数据获得的输入颜色
									"}\0";

const char* fragmentShaderSource =	"#version 330 core\n"//片段着色器1
									"out vec4 FragColor;\n"
									"in vec3 vertexColor;\n"//接收顶点着色器输入的颜色
									"void main()\n"
									"{\n"
									"	FragColor = vec4(vertexColor,1);\n"
									"}\0";

const char* fragmentShaderSource2 =	"#version 330 core\n"
									"out vec4 FragColor;\n"
									"uniform vec4 outColor;\n"
									"void main()\n"
									"{\n"
									"	FragColor = outColor;\n"
									"}\0";

void framebuffer_size_callback(GLFWwindow* window, int width, int height);
void processInput(GLFWwindow* window);

int main() 
{
	glfwInit();//初始化glfw
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);//设置版本号为3
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);//核心模式(Core-profile)

	GLFWwindow* window = glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "MyOpenGLWindow", NULL, NULL);
	if (window == NULL)
	{
		cout << "Failed to create GLFW window" << endl;
		glfwTerminate();

		return -1;
	}

	glfwMakeContextCurrent(window);
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);//每当窗口改变大小，GLFW会调用这个函数

	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))//初始化GLAD
	{
		cout << "Failed to init GLAD" << endl;

		return -1;
	}

#pragma region 顶点着色器

	unsigned int vertexShader;
	vertexShader = glCreateShader(GL_VERTEX_SHADER);//创建一个顶点着色器
	glShaderSource(vertexShader, 1, &vertexShaderSource, NULL);//把着色器源码附加到着色器对象上
	glCompileShader(vertexShader);//编译着色器

	int success;
	char infoLog[512];
	glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);//用于判断着色器编译是否存在错误
	if (!success)
	{
		glGetShaderInfoLog(vertexShader, 512, NULL, infoLog);
		cout << infoLog << endl;
	}

#pragma endregion

#pragma region 片段着色器

	unsigned int fragmentShader;
	fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
	glShaderSource(fragmentShader, 1, &fragmentShaderSource, NULL);
	glCompileShader(fragmentShader);

	glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &success);//用于判断着色器编译是否存在错误
	if (!success)
	{
		glGetShaderInfoLog(fragmentShader, 512, NULL, infoLog);
		cout << infoLog << endl;
	}

	unsigned int fragmentShader2;
	fragmentShader2 = glCreateShader(GL_FRAGMENT_SHADER);
	glShaderSource(fragmentShader2, 1, &fragmentShaderSource2, NULL);
	glCompileShader(fragmentShader2);

#pragma endregion

#pragma region 着色器链接对象

	unsigned int shaderProgram;
	shaderProgram = glCreateProgram();//链接着色器对象，会把每一个着色器的输出链接到下一个着色器的输入。当输出和输入不匹配，则错误；
	
	glAttachShader(shaderProgram, vertexShader);
	glAttachShader(shaderProgram, fragmentShader);
	glLinkProgram(shaderProgram);

	glGetProgramiv(shaderProgram, GL_LINK_STATUS, &success);
	if (!success) 
	{
		glGetProgramInfoLog(shaderProgram, 512, NULL, infoLog);
		cout << infoLog << endl;
	}

	unsigned int shaderProgram2;
	shaderProgram2 = glCreateProgram();
	glAttachShader(shaderProgram2, vertexShader);
	glAttachShader(shaderProgram2, fragmentShader2);
	glLinkProgram(shaderProgram2);

	glDeleteShader(vertexShader);
	glDeleteShader(fragmentShader);
	glDeleteShader(fragmentShader2);

#pragma endregion

#pragma region 顶点输入

	float vertices1[] =
	{
		/*0.5f, 0.5f, 0.0f,   // 右上角
		0.5f, -0.5f, 0.0f,  // 右下角
		-0.5f, -0.5f, 0.0f, // 左下角
		-0.5f, 0.5f, 0.0f   // 左上角*/

		/*0,0,0,
		0.5f, 0, 0,
		0, 0.5f, 0,*/

		// 位置              // 颜色
		0.5f, -0.5f, 0.0f,	 1.0f, 0.0f, 0.0f,   // 右下
		-0.5f, -0.5f, 0.0f,	  0.0f, 1.0f, 0.0f,   // 左下
		0.5f,  0.5f, 0.0f,	 0.0f, 0.0f, 1.0f    // 顶部
	};

	float vertices2[] =
	{
		0.1f,0.1f,0,
		0.6f, 0.1f, 0,
		0.1f, 0.6f, 0,
	};

	unsigned int indices[] =
	{
		//0, 1, 2, // 第一个三角形
		//1, 2, 3  // 第二个三角形
		2,1,0,
		2,0,3
	};

	unsigned int VAO[2];
	unsigned int VBO[2];
	unsigned int EBO;

	glGenVertexArrays(2, VAO);
	glGenBuffers(2, VBO);//做绑定
	glGenBuffers(1, &EBO);

	glBindVertexArray(VAO[0]);
	glBindBuffer(GL_ARRAY_BUFFER, VBO[0]);
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices1), vertices1, GL_STATIC_DRAW);

	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), 0);
	glEnableVertexAttribArray(0);
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)(3 * sizeof(float)));
	glEnableVertexAttribArray(1);

	glBindVertexArray(VAO[1]);
	glBindBuffer(GL_ARRAY_BUFFER, VBO[1]);
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices2), vertices2, GL_STATIC_DRAW);

	/*glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);//把索引复制进缓冲
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);*/

	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
	glEnableVertexAttribArray(0);

#pragma endregion

	/*glBindBuffer(GL_ARRAY_BUFFER, 0);
	glBindVertexArray(0);*/

	//glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);

	string shaderPathSuffix = "D:/Longtu/Graphic_Exercise/graphic_exercise/GLTest/GLTest/Shaders";
	string vertexShaderPath = shaderPathSuffix + "/vertexShader1.glsl";
	string fragmentShaderPath = shaderPathSuffix + "/fragmentShader1.glsl";
	
	const GLchar* vertexSPath = (GLchar*)(vertexShaderPath.c_str());
	const GLchar* fragmentSPath = (GLchar*)(fragmentShaderPath.c_str());
	
	Shader shader(vertexSPath, fragmentSPath);

	while (!glfwWindowShouldClose(window))//渲染循环
	{
		processInput(window);

		glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT);

		shader.Use();
		shader.SetUniformFloat("offset", -0.5f);
		glBindVertexArray(VAO[0]);
		glDrawArrays(GL_TRIANGLES, 0, 3);
		//glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);

		/*glUseProgram(shaderProgram2);

		float timeValue = glfwGetTime();//运行的秒数
		float greenValue = (sin(timeValue) / 2.0f) + 0.5f;
		int vertexColorLocation = glGetUniformLocation(shaderProgram2, "outColor");//定位uniform变量outColor 返回-1代表没找到
		glUniform4f(vertexColorLocation, 0, greenValue, 0, 1);//更新uniform值，必须在更新之前UseProgram

		glBindVertexArray(VAO[1]);
		glDrawArrays(GL_TRIANGLES, 0, 3);*/

		glfwSwapBuffers(window);//交换颜色缓冲（双缓冲）
		glfwPollEvents();//检测触发事件
	}

	glDeleteVertexArrays(2, VAO);
	glDeleteBuffers(2, VBO);
	glDeleteBuffers(1, &EBO);
	glDeleteProgram(shaderProgram);
	glDeleteProgram(shaderProgram2);

	glfwTerminate();//释放资源

	return 0;
}

void framebuffer_size_callback(GLFWwindow* window, int width, int height) 
{
	glViewport(0, 0, width, height);//视口 前两个左下角位置 后两个宽高
}

void processInput(GLFWwindow* window)
{
	if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS) {
		glfwSetWindowShouldClose(window, true);
	}
}