#include<glad/glad.h>
#include<glm/glm.hpp>
#include<glm/gtc/matrix_transform.hpp>

#include<vector>

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