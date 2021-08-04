#include<glad/glad.h>
#include<glm/glm.hpp>
#include<glm/gtc/matrix_transform.hpp>

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

	Vector(float x, float y, float z, float w = 0)
	{
		this->x = x;
		this->y = y;
		this->z = z;
		this->w = w;

		sqrtMagnitude = x * x + y * y + z * z + w + w;
		magnitude = sqrt(sqrtMagnitude);
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

	bool operator == (const Vector &target) const
	{
		if (this->x == target.x && this->y == target.y && this->z == target.z && this->w == target.w)
		{
			return true;
		}

		return false;
	}

private:
	
};

Vector operator+ (const Vector& v1, const Vector& v2)
{
	return Vector(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
}

Vector operator- (const Vector& v1, const Vector& v2)
{
	return Vector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
}

Vector operator* (const Vector& v1, const float x)
{
	return Vector(v1.x * x, v1.y * x, v1.z * x, v1.w * x);
}

#pragma endregion

#pragma region Matrix



#pragma endregion

#pragma region Triangle

class Triangle
{
public:
	Vector point1;
	Vector point2;
	Vector point3;
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
	glm::vec3 position;
	glm::vec3 front;
	glm::vec3 up;
	glm::vec3 right;
	glm::vec3 worldUp;

	float yaw;
	float pitch;
	float movementSpeed;
	float mouseSensitivity;
	float zoom;

	Camera(glm::vec3 position = glm::vec3(0.0f, 0.0f, 0.0f), glm::vec3 up = glm::vec3(0.0f, 1.0f, 0.0f), float yaw = YAW, float pitch = PITCH)
		: front(glm::vec3(0.0f, 0.0f, -1.0f)), movementSpeed(SPEED), mouseSensitivity(SENSITIVITY), zoom(ZOOM)
	{
		this->position = position;
		this->worldUp = up;
		this->yaw = yaw;
		this->pitch = pitch;

		UpdateCameraVectors();
	}

	Camera(float posX, float posY, float posZ, float upX, float upY, float upZ, float yaw, float pitch)
		: front(glm::vec3(0.0f, 0.0f, -1.0f)), movementSpeed(SPEED), mouseSensitivity(SENSITIVITY), zoom(ZOOM)
	{
		this->position = glm::vec3(posX, posY, posZ);
		this->worldUp = glm::vec3(upX, upY, upZ);
		this->yaw = yaw;
		this->pitch = pitch;

		UpdateCameraVectors();
	}

	glm::mat4 GetViewMatrix()
	{
		return glm::lookAt(position, position + front, up);
	}

	void ProcessKeyboard(Camera_Movement_Type direction, float deltaTime)
	{
		float velocity = movementSpeed * deltaTime;
		if (direction == Forward)
		{
			position += front * velocity;
		}
		if (direction == Backward)
		{
			position -= front * velocity;
		}
		if (direction == Left)
		{
			position -= right * velocity;
		}
		if (direction == Right)
		{
			position += right * velocity;
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

private:
	void UpdateCameraVectors()
	{
		glm::vec3 front;

		front.x = cos(glm::radians(yaw)) * cos(glm::radians(pitch));
		front.y = sin(glm::radians(pitch));
		front.z = sin(glm::radians(yaw)) * cos(glm::radians(pitch));
		this->front = glm::normalize(front);

		right = glm::normalize(glm::cross(this->front, worldUp));
		up = glm::normalize(glm::cross(right, this->front));
	}

	glm::mat4 LookAtMatrix(glm::vec3 position, glm::vec3 target, glm::vec3 worldUp)
	{
		glm::vec3 zaxis = glm::normalize(position - target);
		glm::vec3 xaxis = glm::normalize(glm::cross(glm::normalize(worldUp), zaxis));
		glm::vec3 yaxis = glm::cross(zaxis, xaxis);
	}
};

#pragma endregion

class Custom
{
public:
	

private:

};