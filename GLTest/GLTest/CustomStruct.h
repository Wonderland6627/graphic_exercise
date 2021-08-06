#include<glad/glad.h>
#include<glm/glm.hpp>
#include<glm/gtc/matrix_transform.hpp>
#include<GLFW/glfw3.h>

#pragma region Vector

class Vector
{
public:
	float x;
	float y;
	float z;
	float w;

	float magnitude;//模
	float sqrtMagnitude;//模的平方

	Vector()
	{

	}

	Vector(float x, float y, float z, float w = 0)
	{
		this->x = x;
		this->y = y;
		this->z = z;
		this->w = w;

		sqrtMagnitude = x * x + y * y + z * z + w + w;
		magnitude = sqrt(sqrtMagnitude);
	}

	glm::vec3 Toglmvec3()
	{
		glm::vec3 v(this->x, this->y, this->z);

		return v;
	}

	string ToString()
	{
		ostringstream ostr;
		ostr << "x:" << x << " y:" << y << " z:" << z << " w:" << w;

		return ostr.str();
	}

	static Vector right()
	{
		return Vector(1, 0, 0);
	}

	static Vector up()
	{
		return Vector(0, 1, 0);
	}

	static Vector forward()
	{
		return Vector(0, 0, 1);
	}

	static float Dot(Vector v1, Vector v2)
	{
		float result = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z + v1.w * v2.w;

		return result;
	}

	static Vector Cross(Vector v1, Vector v2)
	{
		float x = v1.y * v2.z - v1.z * v2.y;
		float y = v1.z * v2.x - v1.x * v2.z;
		float z = v1.x * v2.y - v1.y * v2.x;

		return Vector(x, y, z);
	}

	static Vector ToVector(glm::vec3 v)
	{
		return Vector(v.x, v.y, v.z);
	}

	bool operator == (const Vector& target) const
	{
		if (this->x == target.x && this->y == target.y && this->z == target.z && this->w == target.w)
		{
			return true;
		}

		return false;
	}

private:

};

Vector operator + (const Vector& v1, const Vector& v2)
{
	return Vector(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
}

Vector operator - (const Vector& v1, const Vector& v2)
{
	return Vector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
}

Vector operator * (const Vector& v1, const float x)
{
	return Vector(v1.x * x, v1.y * x, v1.z * x, v1.w * x);
}

#pragma endregion

#pragma region Matrix

class Matrix
{
public:
	int rows;
	int columns;
	int size;

	double** elements;

	Matrix()
	{

	}

	Matrix(int rows, int columns, double value = 0)
	{
		this->rows = rows;
		this->columns = columns;
		size = rows * columns;

		if (size > 0)
		{
			elements = new double* [rows];
			for (int i = 0; i < rows; i++)
			{
				elements[i] = new double[columns];
			}

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					elements[i][j] = value;
				}
			}
		}
	}

	string ToString()
	{
		string log = "";
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				cout << elements[i][j] << "\t";
			}
			cout << endl;
		}

		return log;
	}
};

Matrix operator + (const Matrix& m1, const Matrix& m2)
{
	if (m1.rows != m2.rows || m1.columns != m2.columns)
	{
		Matrix emptyMatrix(0, 0);
		cout << "矩阵格式不对，不能相加。" << endl;

		return emptyMatrix;
	}

	Matrix resultMatrix(m1.rows, m1.columns);
	for (int i = 0; i < m1.rows; i++)
	{
		for (int j = 0; j < m1.columns; j++)
		{
			resultMatrix.elements[i][j] = m1.elements[i][j] + m2.elements[i][j];
		}
	}

	return resultMatrix;
}

Matrix operator - (const Matrix& m1, const Matrix& m2)
{
	if (m1.rows != m2.rows || m1.columns != m2.columns)
	{
		Matrix emptyMatrix(0, 0);
		cout << "矩阵格式不对，不能相减。" << endl;

		return emptyMatrix;
	}

	Matrix resultMatrix(m1.rows, m1.columns);
	for (int i = 0; i < m1.rows; i++)
	{
		for (int j = 0; j < m1.columns; j++)
		{
			resultMatrix.elements[i][j] = m1.elements[i][j] - m2.elements[i][j];
		}
	}

	return resultMatrix;
}

Matrix operator * (const Matrix& m1, const Matrix& m2)
{
	if (m1.columns != m2.rows)
	{
		Matrix emptyMatrix(0, 0);
		cout << "矩阵格式不对，不能相乘。（m1的列数要和m2的行数相等）" << endl;

		return emptyMatrix;
	}

	Matrix resultMatrix(m1.rows, m1.columns);

	for (int i = 0; i < m1.rows; i++)
	{
		for (int j = 0; j < m2.columns; j++)
		{
			for (int k = 0; k < m1.columns; k++)
			{
				resultMatrix.elements[i][j] += (m1.elements[i][k] * m2.elements[k][j]);
				//第i行j列的值为m1的第i行上的n个数和m2的第j列上的n个数对应相乘之和，其中n为m1的列数，也是m2的行数，m1的列数和m2的行数相等
			}
		}
	}

	return resultMatrix;
}

#pragma endregion

#pragma region Texture2D

class Texture2D
{
public:
	GLuint ID;
	unsigned int Width;
	unsigned int Height;
	unsigned int Internal_Format;
	unsigned int Image_Format;
	unsigned int Wrap_S;
	unsigned int Wrap_T;
	unsigned int Filter_Min;
	unsigned int Filter_Max;

	Texture2D()
		: Width(0), Height(0)
		, Internal_Format(GL_RGB), Image_Format(GL_RGB)
		, Wrap_S(GL_REPEAT), Wrap_T(GL_REPEAT)
		, Filter_Min(GL_LINEAR), Filter_Max(GL_LINEAR)
	{
		glGenTextures(1, &ID);
	}

	void Generate(unsigned int width, unsigned int height, unsigned char* data)
	{
		this->Width = width;
		this->Height = height;

		glBindTexture(GL_TEXTURE_2D, ID);
		glTexImage2D(GL_TEXTURE_2D, 0, this->Internal_Format, width, height, 0, this->Image_Format, GL_UNSIGNED_BYTE, data);

		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, this->Wrap_S);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, this->Wrap_T);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, this->Filter_Min);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, this->Filter_Max);

		glBindTexture(GL_TEXTURE_2D, 0);
	}

	void Bind() const
	{
		glBindTexture(GL_TEXTURE_2D, ID);
	}
private:

};

#pragma endregion

#pragma region Camera

enum Camera_Movement_Type
{
	Forward,
	Backward,
	Left,
	Right,
};

const float YAW = -90.0f;
const float PITCH = 0.0f;
const float SPEED = 2.5f;
const float SENSITIVITY = 0.1f;
const float ZOOM = 45.0f;

class Camera
{
public:
	Vector position;
	Vector front;
	Vector up;
	Vector right;
	Vector worldUp;

	float yaw;
	float pitch;
	float movementSpeed;
	float mouseSensitivity;
	float zoom;

	Camera(Vector position = Vector(0.0f, 0.0f, 0.0f), Vector up = Vector(0.0f, 1.0f, 0.0f), float yaw = YAW, float pitch = PITCH)
		: front(Vector(0.0f, 0.0f, -1.0f)), movementSpeed(SPEED), mouseSensitivity(SENSITIVITY), zoom(ZOOM)
	{
		this->position = position;
		this->worldUp = up;
		this->yaw = yaw;
		this->pitch = pitch;

		UpdateCameraVectors();
	}

	Camera(float posX, float posY, float posZ, float upX, float upY, float upZ, float yaw, float pitch)
		: front(Vector(0.0f, 0.0f, -1.0f)), movementSpeed(SPEED), mouseSensitivity(SENSITIVITY), zoom(ZOOM)
	{
		this->position = Vector(posX, posY, posZ);
		this->worldUp = Vector(upX, upY, upZ);
		this->yaw = yaw;
		this->pitch = pitch;

		UpdateCameraVectors();
	}

	glm::mat4 GetViewMatrix()
	{
		return glm::lookAt(position.Toglmvec3(), position.Toglmvec3() + front.Toglmvec3(), up.Toglmvec3());
	}

	void ProcessKeyboard(Camera_Movement_Type direction, float deltaTime)
	{
		float velocity = movementSpeed * deltaTime;
		if (direction == Forward)
		{
			position = position + front * velocity;
		}
		if (direction == Backward)
		{
			position = position - front * velocity;
		}
		if (direction == Left)
		{
			position = position - right * velocity;
		}
		if (direction == Right)
		{
			position = position + right * velocity;
		}
	}

	void ProcessMouseMovement(float xOffset, float yOffset, GLboolean constrainPitch = true)
	{
		xOffset *= mouseSensitivity;
		yOffset *= mouseSensitivity;

		yaw += xOffset;
		pitch += yOffset;

		if (constrainPitch)
		{
			if (pitch > 89.0f)
			{
				pitch = 89.0f;
			}
			if (pitch < -89.0f)
			{
				pitch = -89.0f;
			}
		}

		UpdateCameraVectors();
	}

	void ProcessMouseScroll(float yOffset)
	{
		zoom -= (float)yOffset;
		if (zoom < 1.0f)
		{
			zoom = 1.0f;
		}
		if (zoom > 45.0f)
		{
			zoom = 45.0f;
		}
	}

	void ListenMoveInput(GLFWwindow* window, float deltaTime)
	{
		if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
		{
			ProcessKeyboard(Forward, deltaTime);
		}
		if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
		{
			ProcessKeyboard(Backward, deltaTime);
		}
		if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
		{
			ProcessKeyboard(Left, deltaTime);
		}
		if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
		{
			ProcessKeyboard(Right, deltaTime);
		}
	}

private:
	void UpdateCameraVectors()
	{
		glm::vec3 front;

		front.x = cos(glm::radians(yaw)) * cos(glm::radians(pitch));
		front.y = sin(glm::radians(pitch));
		front.z = sin(glm::radians(yaw)) * cos(glm::radians(pitch));
		this->front = Vector::ToVector(glm::normalize(front));

		glm::vec3 glmRight = glm::normalize(glm::cross(this->front.Toglmvec3(), worldUp.Toglmvec3()));
		glm::vec3 glmUp = glm::normalize(glm::cross(right.Toglmvec3(), this->front.Toglmvec3()));

		right = Vector::ToVector(glmRight);
		up = Vector::ToVector(glmUp);
	}
};

#pragma endregion

#pragma region Triangle

class Triangle
{
public:

	Shader shader;
	Texture2D texture;
	GLuint VAO;

	Triangle(Shader& shader, Texture2D& texture, float vertice[] = NULL)
	{
		this->shader = shader;
		this->texture = texture;
		InitTriangle(vertice);
	}

	void InitTriangle(float vertice[])
	{
		GLuint VBO;
		GLfloat vertices[] =
		{
			// 位置				  // 纹理       // 法线
			-0.5f, -0.5f, -0.5f,  0.0f, 0.0f,	0.0f,  0.0f, -1.0f,
			 0.5f, -0.5f, -0.5f,  1.0f, 0.0f,	0.0f,  0.0f, -1.0f,
			 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,	0.0f,  0.0f, -1.0f,
			 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,	0.0f,  0.0f, -1.0f,
			-0.5f,  0.5f, -0.5f,  0.0f, 1.0f,	0.0f,  0.0f, -1.0f,
			-0.5f, -0.5f, -0.5f,  0.0f, 0.0f,	0.0f,  0.0f, -1.0f,

			-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,	0.0f,  0.0f, 1.0f,
			 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,	0.0f,  0.0f, 1.0f,
			 0.5f,  0.5f,  0.5f,  1.0f, 1.0f,	0.0f,  0.0f, 1.0f,
			 0.5f,  0.5f,  0.5f,  1.0f, 1.0f,	0.0f,  0.0f, 1.0f,
			-0.5f,  0.5f,  0.5f,  0.0f, 1.0f,	0.0f,  0.0f, 1.0f,
			-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,	0.0f,  0.0f, 1.0f,

			-0.5f,  0.5f,  0.5f,  1.0f, 0.0f,	-1.0f,  0.0f,  0.0f,
			-0.5f,  0.5f, -0.5f,  1.0f, 1.0f,	-1.0f,  0.0f,  0.0f,
			-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,	-1.0f,  0.0f,  0.0f,
			-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,	-1.0f,  0.0f,  0.0f,
			-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,	-1.0f,  0.0f,  0.0f,
			-0.5f,  0.5f,  0.5f,  1.0f, 0.0f,	-1.0f,  0.0f,  0.0f,

			 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,	1.0f,  0.0f,  0.0f,
			 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,	1.0f,  0.0f,  0.0f,
			 0.5f, -0.5f, -0.5f,  0.0f, 1.0f,	1.0f,  0.0f,  0.0f,
			 0.5f, -0.5f, -0.5f,  0.0f, 1.0f,	1.0f,  0.0f,  0.0f,
			 0.5f, -0.5f,  0.5f,  0.0f, 0.0f,	1.0f,  0.0f,  0.0f,
			 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,	1.0f,  0.0f,  0.0f,

			-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,	0.0f, -1.0f,  0.0f,
			 0.5f, -0.5f, -0.5f,  1.0f, 1.0f,	0.0f, -1.0f,  0.0f,
			 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,	0.0f, -1.0f,  0.0f,
			 0.5f, -0.5f,  0.5f,  1.0f, 0.0f,	0.0f, -1.0f,  0.0f,
			-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,	0.0f, -1.0f,  0.0f,
			-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,	0.0f, -1.0f,  0.0f,

			-0.5f,  0.5f, -0.5f,  0.0f, 1.0f,	0.0f,  1.0f,  0.0f,
			 0.5f,  0.5f, -0.5f,  1.0f, 1.0f,	0.0f,  1.0f,  0.0f,
			 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,	0.0f,  1.0f,  0.0f,
			 0.5f,  0.5f,  0.5f,  1.0f, 0.0f,	0.0f,  1.0f,  0.0f,
			-0.5f,  0.5f,  0.5f,  0.0f, 0.0f,	0.0f,  1.0f,  0.0f,
			-0.5f,  0.5f, -0.5f,  0.0f, 1.0f,	0.0f,  1.0f,  0.0f,
		};

		glGenVertexArrays(1, &VAO);
		glGenBuffers(1, &VBO);

		glBindBuffer(GL_ARRAY_BUFFER, VBO);
		glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

		glBindVertexArray(VAO);
		glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)0);
		glEnableVertexAttribArray(0);
		glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(3 * sizeof(float)));
		glEnableVertexAttribArray(1);
		glVertexAttribPointer(2, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(5 * sizeof(float)));
		glEnableVertexAttribArray(2);
	}

	void Draw(Camera& camera)
	{
		glm::mat4 projection = glm::perspective(glm::radians(camera.zoom), (float)800 / (float)800, 0.1f, 100.0f);
		glm::mat4 view = camera.GetViewMatrix();
		glm::mat4 model = glm::mat4(1.0f);

		shader.Use();
		shader.SetUniformInt("texture1", 0);
		shader.SetUniformMatrix4fv("projection", projection);
		shader.SetUniformMatrix4fv("view", view);
		shader.SetUniformMatrix4fv("model", model);

		glActiveTexture(GL_TEXTURE0);
		texture.Bind();

		glBindVertexArray(VAO);
		glDrawArrays(GL_TRIANGLES, 0, 36);
		glBindVertexArray(0);
	}

	void DrawLine(Camera& camera)
	{
		glm::mat4 projection = glm::perspective(glm::radians(camera.zoom), (float)800 / (float)800, 0.1f, 100.0f);
		glm::mat4 view = camera.GetViewMatrix();
		glm::mat4 model = glm::mat4(1.0f);

		shader.Use();
		shader.SetUniformInt("texture1", 0);
		shader.SetUniformMatrix4fv("projection", projection);
		shader.SetUniformMatrix4fv("view", view);
		shader.SetUniformMatrix4fv("model", model);

		glBindVertexArray(VAO);
		glLineWidth(2);
		glDrawArrays(GL_LINE_LOOP, 0, 36);
	}

	void ColorDraw(Camera& camera)
	{
		shader.Use();
		shader.SetUniformInt("texture1", 0);

		shader.SetUniformVec3("objectColor", 1.0f, 1.0f, 1.0f);
		shader.SetUniformVec3("lightColor", 1.0f, 1.0f, 1.0f);
		shader.SetUniformVec3("lightPos", 1.5f, 1.5f, 1.5f);
		shader.SetUniformVec3("viewPos", camera.position.Toglmvec3());

		glm::mat4 projection = glm::perspective(glm::radians(camera.zoom), (float)800 / (float)800, 0.1f, 100.0f);
		glm::mat4 view = camera.GetViewMatrix();
		glm::mat4 model = glm::mat4(1.0f);
		shader.SetUniformMatrix4fv("projection", projection);
		shader.SetUniformMatrix4fv("view", view);
		shader.SetUniformMatrix4fv("model", model);

		glActiveTexture(GL_TEXTURE0);
		texture.Bind();

		glBindVertexArray(VAO);
		glDrawArrays(GL_TRIANGLES, 0, 36);
	}

private:

};

#pragma endregion

class Custom
{
public:


private:

};