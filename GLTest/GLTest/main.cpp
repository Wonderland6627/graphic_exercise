#include<glad/glad.h>
#include<GLFW/glfw3.h>
#include<iostream>
#include<string>
#include<algorithm>

#include<glm/glm.hpp>
#include<glm/gtc/matrix_transform.hpp>
#include<glm/gtc/type_ptr.hpp>

#include"Shader.h"
#include"CustomStruct.h"

using namespace std;

#pragma region GLTestConst

const unsigned int SCR_WIDTH = 800;
const unsigned int SCR_HEIGHT = 800;

const char* vertexShaderSource = "#version 330 core\n"//顶点着色器
								 "layout(location = 0) in vec3 aPos;\n"//位置变量的属性位置值为0
								 "layout(location = 1) in vec3 aColor;\n"//颜色变量的属性位置值为1
								 "out vec3 vertexColor;\n"//向片段着色器输出一个颜色 接收要同名同类型
								 "void main()\n"
								 "{\n"
								 "	gl_Position = vec4(aPos, 1.0);\n"
								 "	vertexColor = aColor;\n"//将vertexColor设置为从顶点数据获得的输入颜色
								 "}\0";

const char* fragmentShaderSource = "#version 330 core\n"//片段着色器1
								   "out vec4 FragColor;\n"
								   "in vec3 vertexColor;\n"//接收顶点着色器输入的颜色
								   "void main()\n"
								   "{\n"
								   "	FragColor = vec4(vertexColor,1);\n"
								   "}\0";

const char* fragmentShaderSource2 = "#version 330 core\n"
									"out vec4 FragColor;\n"
									"uniform vec4 outColor;\n"
									"void main()\n"
									"{\n"
									"	FragColor = outColor;\n"
									"}\0";

#pragma endregion

float lastX = 400;
float lastY = 400;
float yaw = 0;
float pitch = 0;
glm::vec3 cameraPos = glm::vec3(0.0f, 0.0f, 3.0f);
glm::vec3 cameraFront = glm::vec3(0.0f, 0.0f, -1.0f);
glm::vec3 cameraUp = glm::vec3(0.0f, 1.0f, 0.0f);

void Framebuffer_size_callback(GLFWwindow* window, int width, int height);
void Mouse_Callback(GLFWwindow* window, double xpos, double ypos);
void ProcessInput(GLFWwindow* window);
void ClearScreen();
void GLTestFunc();
void GLTextureTestFunc();
void GLMTest();
void GLSpaceTest();

int main()
{
	//GLTestFunc();
	//GLTextureTestFunc();
	//GLMTest();
	GLSpaceTest();

	return 0;
}

void Framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
	glViewport(0, 0, width, height);//视口 前两个左下角位置 后两个宽高
}

void ProcessInput(GLFWwindow* window)
{
	if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
	{
		glfwSetWindowShouldClose(window, true);
	}
}

void ClearScreen()
{
	glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
}

GLFWwindow* InitGlfwWindow()
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

		return NULL;
	}

	glfwMakeContextCurrent(window);
	glfwSetFramebufferSizeCallback(window, Framebuffer_size_callback);//每当窗口改变大小，GLFW会调用这个函数

	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))//初始化GLAD
	{
		cout << "Failed to init GLAD" << endl;

		return NULL;
	}

	return window;
}

void GLTestFunc()
{
	GLFWwindow* window = InitGlfwWindow();
	if (window == NULL)
	{
		cout << "GLFWWindow is null" << endl;

		return;
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
		ProcessInput(window);

		ClearScreen();

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
}

#define STB_IMAGE_IMPLEMENTATION
#include"stb_image.h"

void GLTextureTestFunc()
{
	GLFWwindow* window = InitGlfwWindow();
	if (window == NULL)
	{
		cout << "GLFWWindow is null" << endl;

		return;
	}

	float texCoords[] =
	{
		0.0f, 0.0f, // 左下角
		1.0f, 0.0f, // 右下角
		0.5f, 1.0f // 上中
	};

	float vertices[] =
	{
		//     ---- 位置 ----       ---- 颜色 ----     - 纹理坐标 -
			 0.5f,  0.5f, 0.0f,   1.0f, 0.0f, 0.0f,   1.0f, 1.0f,   // 右上
			 0.5f, -0.5f, 0.0f,   0.0f, 1.0f, 0.0f,   1.0f, 0.0f,   // 右下
			-0.5f, -0.5f, 0.0f,   0.0f, 0.0f, 1.0f,   0.0f, 0.0f,   // 左下
			-0.5f,  0.5f, 0.0f,   1.0f, 1.0f, 0.0f,   0.0f, 1.0f    // 左上
	};

	unsigned int indices[] =
	{
		0,1,3,
		1,2,3,
	};

	unsigned int VAO;
	unsigned int VBO;
	unsigned int EBO;

	glGenVertexArrays(1, &VAO);
	glBindVertexArray(VAO);

	glGenBuffers(1, &VBO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

	glGenBuffers(1, &EBO);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);

	//位置属性
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), 0);
	glEnableVertexAttribArray(0);
	//颜色属性
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(3 * sizeof(float)));
	glEnableVertexAttribArray(1);
	//贴图属性
	glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(6 * sizeof(float)));
	glEnableVertexAttribArray(2);

	unsigned int texture1;

	glGenTextures(1, &texture1);
	glBindTexture(GL_TEXTURE_2D, texture1);//绑定贴图

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	int width;
	int height;
	int nrChannels;//颜色通道数
	unsigned char* data = stbi_load("D:/Longtu/Graphic_Exercise/graphic_exercise/GLTest/GLTest/Textures/wall.jpg", &width, &height, &nrChannels, 0);//加载贴图数据

	if (data)
	{
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, data);
		glGenerateMipmap(GL_TEXTURE_2D);//生成纹理
	}
	else
	{
		cout << "Load Texture1 Failed" << endl;
	}

	stbi_image_free(data);

	unsigned int texture2;

	glGenTextures(1, &texture2);
	glBindTexture(GL_TEXTURE_2D, texture2);

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	int width2;
	int height2;
	int nrChannels2;
	stbi_set_flip_vertically_on_load(true);
	unsigned char* data2 = stbi_load("D:/Longtu/Graphic_Exercise/graphic_exercise/GLTest/GLTest/Textures/awesomeface.png", &width2, &height2, &nrChannels2, 0);//加载贴图数据

	if (data2)
	{
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data2);
		glGenerateMipmap(GL_TEXTURE_2D);//生成纹理
	}
	else
	{
		cout << "Load Texture2 Failed" << endl;
	}

	stbi_image_free(data2);

	string shaderPathSuffix = "D:/Longtu/Graphic_Exercise/graphic_exercise/GLTest/GLTest/Shaders";
	string vertexShaderPath = shaderPathSuffix + "/TextureVertex.glsl";
	string fragmentShaderPath = shaderPathSuffix + "/TextureFragment.glsl";

	const GLchar* vertexSPath = (GLchar*)(vertexShaderPath.c_str());
	const GLchar* fragmentSPath = (GLchar*)(fragmentShaderPath.c_str());

	Shader shader(vertexSPath, fragmentSPath);
	shader.Use();
	shader.SetUniformInt("texture1", 0);
	shader.SetUniformInt("texture2", 1);
	float alpha = 0.1f;
	shader.SetUniformFloat("alpha", alpha);

	glm::mat4 trans;
	trans = glm::rotate(trans, (float)glfwGetTime(), glm::vec3(0, 0, 1));
	trans = glm::scale(trans, glm::vec3(0.5f, 0.5f, 0.5f));
	unsigned int transformLoc = glGetUniformLocation(shader.ID, "transform");
	glUniformMatrix4fv(transformLoc, 1, GL_FALSE, glm::value_ptr(trans));

	while (!glfwWindowShouldClose(window))//渲染循环
	{
		ProcessInput(window);

		ClearScreen();

		glActiveTexture(GL_TEXTURE0);//在绑定纹理之前先激活纹理单元 GL_TEXTURE0纹理单元默认总是被激活的
		glBindTexture(GL_TEXTURE_2D, texture1);
		glActiveTexture(GL_TEXTURE1);
		glBindTexture(GL_TEXTURE_2D, texture2);

		shader.Use();
		if (glfwGetKey(window, GLFW_KEY_UP) == GLFW_PRESS)
		{
			alpha += 0.1f;
			shader.SetUniformFloat("alpha", alpha);
		}
		else if (glfwGetKey(window, GLFW_KEY_DOWN) == GLFW_PRESS)
		{
			alpha -= 0.1f;
			shader.SetUniformFloat("alpha", alpha);
		}

		glm::mat4 trans;
		trans = glm::translate(trans, glm::vec3(0.5f, -0.5f, 0));
		trans = glm::rotate(trans, (float)glfwGetTime(), glm::vec3(0, 0, 1));
		unsigned int transformLoc = glGetUniformLocation(shader.ID, "transform");
		glUniformMatrix4fv(transformLoc, 1, GL_FALSE, glm::value_ptr(trans));

		glBindVertexArray(VAO);
		glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);

		trans = glm::mat4(1.0f); // reset it to identity matrix
		trans = glm::translate(trans, glm::vec3(-0.5f, 0.5f, 0.0f));
		float scaleAmount = sin(glfwGetTime());
		if (scaleAmount < 0)
		{
			scaleAmount = -scaleAmount;
		}
		trans = glm::scale(trans, glm::vec3(scaleAmount, scaleAmount, scaleAmount));
		glUniformMatrix4fv(transformLoc, 1, GL_FALSE, glm::value_ptr(trans));

		glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);

		glfwSwapBuffers(window);//交换颜色缓冲（双缓冲）
		glfwPollEvents();//检测触发事件
	}

	glDeleteVertexArrays(1, &VAO);
	glDeleteBuffers(1, &VBO);
	glDeleteBuffers(1, &EBO);

	glfwTerminate();//释放资源
}

void GLMTest()
{
	glm::vec4 vec(1, 0, 0, 1);
	glm::mat4 trans;
	trans = glm::translate(trans, glm::vec3(1, 1, 0));
	vec = trans * vec;
	cout << vec.x << vec.y << vec.z << endl;
}

void GLSpaceTest()
{
	GLFWwindow* window = InitGlfwWindow();
	if (window == NULL)
	{
		cout << "GLFWWindow is null" << endl;

		return;
	}

	glEnable(GL_DEPTH_TEST);

	glm::vec3 cubePositions[] =
	{
		glm::vec3(0.0f,  0.0f,  0.0f),
		glm::vec3(2.0f,  5.0f, -15.0f),
		glm::vec3(-1.5f, -2.2f, -2.5f),
		glm::vec3(-3.8f, -2.0f, -12.3f),
		glm::vec3(2.4f, -0.4f, -3.5f),
		glm::vec3(-1.7f,  3.0f, -7.5f),
		glm::vec3(1.3f, -2.0f, -2.5f),
		glm::vec3(1.5f,  2.0f, -2.5f),
		glm::vec3(1.5f,  0.2f, -1.5f),
		glm::vec3(-1.3f,  1.0f, -1.5f)
	};

	float vertices[] =
	{
		-0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
		 0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
		 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
		 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
		-0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
		-0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

		-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
		 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
		 0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
		 0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
		-0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
		-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

		-0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
		-0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
		-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
		-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
		-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
		-0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

		 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
		 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
		 0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
		 0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
		 0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
		 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

		-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
		 0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
		 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
		 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
		-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
		-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

		-0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
		 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
		 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
		 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
		-0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
		-0.5f,  0.5f, -0.5f,  0.0f, 1.0f
	};

	unsigned int indices[] =
	{
		0,1,3,
		1,2,3,
	};

	unsigned int VAO;
	unsigned int VBO;

	glGenVertexArrays(1, &VAO);
	glBindVertexArray(VAO);

	glGenBuffers(1, &VBO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(float), 0);
	glEnableVertexAttribArray(0);

	glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)(3 * sizeof(float)));
	glEnableVertexAttribArray(1);

	unsigned int texture1;

	glGenTextures(1, &texture1);
	glBindTexture(GL_TEXTURE_2D, texture1);//绑定贴图

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	int width;
	int height;
	int nrChannels;//颜色通道数
	unsigned char* data = stbi_load("D:/Longtu/Graphic_Exercise/graphic_exercise/GLTest/GLTest/Textures/wall.jpg", &width, &height, &nrChannels, 0);//加载贴图数据

	if (data)
	{
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, data);
		glGenerateMipmap(GL_TEXTURE_2D);//生成纹理
	}
	else
	{
		cout << "Load Texture1 Failed" << endl;
	}

	stbi_image_free(data);

	unsigned int texture2;

	glGenTextures(1, &texture2);
	glBindTexture(GL_TEXTURE_2D, texture2);

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	int width2;
	int height2;
	int nrChannels2;
	stbi_set_flip_vertically_on_load(true);
	unsigned char* data2 = stbi_load("D:/Longtu/Graphic_Exercise/graphic_exercise/GLTest/GLTest/Textures/awesomeface.png", &width2, &height2, &nrChannels2, 0);//加载贴图数据

	if (data2)
	{
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data2);
		glGenerateMipmap(GL_TEXTURE_2D);//生成纹理
	}
	else
	{
		cout << "Load Texture2 Failed" << endl;
	}

	stbi_image_free(data2);

	string shaderPathSuffix = "D:/Longtu/Graphic_Exercise/graphic_exercise/GLTest/GLTest/Shaders";
	string vertexShaderPath = shaderPathSuffix + "/SpaceVertex.glsl";
	string fragmentShaderPath = shaderPathSuffix + "/SpaceFragment.glsl";

	const GLchar* vertexSPath = (GLchar*)(vertexShaderPath.c_str());
	const GLchar* fragmentSPath = (GLchar*)(fragmentShaderPath.c_str());

	Shader shader(vertexSPath, fragmentSPath);
	shader.Use();
	shader.SetUniformInt("texture1", 0);
	shader.SetUniformInt("texture2", 1);
	shader.SetUniformFloat("alpha", 0.2f);

	float offsetX = 0;
	float offsetZ = 0;

	float cameraSpeed = 0.1f;

	while (!glfwWindowShouldClose(window))//渲染循环
	{
		ProcessInput(window);

		float deltaTime = 0;
		float lastFrame = 0;
		float currentFrame = glfwGetTime();
		deltaTime = currentFrame - lastFrame;
		lastFrame = currentFrame;

		cameraSpeed = 0.1f * deltaTime;

		if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
		{
			cameraPos += cameraSpeed * cameraFront;
		}
		if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
		{
			cameraPos -= cameraSpeed * cameraFront;
		}
		if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
		{
			cameraPos -= glm::normalize(glm::cross(cameraFront, cameraUp)) * cameraSpeed;
		}
		if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
		{
			cameraPos += glm::normalize(glm::cross(cameraFront, cameraUp)) * cameraSpeed;
		}

		glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);

		ClearScreen();

		glActiveTexture(GL_TEXTURE0);//在绑定纹理之前先激活纹理单元 GL_TEXTURE0纹理单元默认总是被激活的
		glBindTexture(GL_TEXTURE_2D, texture1);
		glActiveTexture(GL_TEXTURE1);
		glBindTexture(GL_TEXTURE_2D, texture2);

		shader.Use();

		glBindVertexArray(VAO);

		for (unsigned int i = 0; i < 10; i++)
		{
			float angle = 20.0f * i + 15;
			glm::mat4 model;
			glm::mat4 view;
			glm::mat4 projection;

			model = glm::translate(model, cubePositions[i]);
			if (i % 3 != 0 || i == 0)
			{
				model = glm::rotate(model, glm::radians(angle) * (float)glfwGetTime(), glm::vec3(1, 0.0f, 0.5f));
			}
			//view = glm::translate(view, glm::vec3(0.0f + offsetX, 0.0f, -3.0f + offsetZ));

			float radius = 10;
			float camX = sin(glfwGetTime()) * radius;
			float camZ = cos(glfwGetTime()) * radius;
			view = glm::lookAt(cameraPos, cameraPos + cameraFront, cameraUp);

			projection = glm::perspective(glm::radians(45.0f), (float)SCR_WIDTH / (float)SCR_HEIGHT, 0.1f, 100.0f);

			shader.SetUniformMatrix4fv("model", model);
			shader.SetUniformMatrix4fv("view", view);
			shader.SetUniformMatrix4fv("projection", projection);

			glDrawArrays(GL_TRIANGLES, 0, 36);
		}

		glfwSwapBuffers(window);//交换颜色缓冲（双缓冲）
		glfwPollEvents();//检测触发事件
	}

	glDeleteVertexArrays(1, &VAO);
	glDeleteBuffers(1, &VBO);

	glfwTerminate();//释放资源
}

void GLCameraTest() 
{
	glm::vec3 cameraPos = glm::vec3(0, 0, 3);//摄像机位置
	glm::vec3 cameraTarget = glm::vec3(0, 0, 0);//摄像机朝向
	glm::vec3 cameraDirection = glm::normalize(cameraPos - cameraTarget);//摄像机方向

	glm::vec3 up = glm::vec3(0, 1, 0);
	glm::vec3 cameraRight = glm::normalize(glm::cross(up, cameraDirection));//右轴
	glm::vec3 cameraUp = glm::cross(cameraDirection, cameraRight);//上轴

	glm::mat4 view;
	view = glm::lookAt(glm::vec3(0, 0, 3), glm::vec3(0, 0, 0), glm::vec3(0, 1, 0));
}

void Mouse_Callback(GLFWwindow* window, double xpos, double ypos)
{
	if (true)
	{
		lastX = xpos;
		lastY = ypos;
	}
	
	float xOffset = xpos - lastX;
	float yOffset = lastY - ypos;

	lastX = xpos;
	lastY = ypos;

	float sensitivity = 0.05f;//灵敏度
	xOffset *= sensitivity;
	yOffset *= sensitivity;

	yaw += xOffset;
	pitch = yOffset;

	if (pitch > 89.0f)
	{
		pitch = 89.0f;
	}
	if (pitch < -89.0f)
	{
		pitch = -89.0f;
	}

	glm::vec3 front;
	front.x = cos(glm::radians(pitch)) * cos(glm::radians(yaw));
	front.y = sin(glm::radians(pitch));
	front.z = cos(glm::radians(pitch)) * sin(glm::radians(yaw));

	cameraFront = glm::normalize(front);
}