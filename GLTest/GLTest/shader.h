#ifndef SHADER_H

#define SHADER_H

#include<glad/glad.h>

#include<string>
#include<fstream>
#include<sstream>
#include<iostream>

using namespace std;

class Shader 
{
public:
		unsigned int ID;//程序ID

		Shader(const GLchar* vertexPath, const GLchar* fragmentPath)
		{
			string vertexCode;
			string fragmentCode;
			ifstream vShaderFile;//ifstream 从硬盘到内存
			ifstream fShaderFile;

			vShaderFile.exceptions(ifstream::failbit | ifstream::badbit);//保证ifstream对象可以抛出异常
			fShaderFile.exceptions(ifstream::failbit | ifstream::badbit);

			try 
			{
				vShaderFile.open(vertexPath);
				fShaderFile.open(fragmentPath);

				stringstream vShaderStream;
				stringstream fShaderStream;

				vShaderStream << vShaderFile.rdbuf();//读取文件的缓冲内容到数据流中 <<插入器
				fShaderStream << fShaderFile.rdbuf();

				vShaderFile.close();
				fShaderFile.close();

				vertexCode = vShaderStream.str();
				fragmentCode = fShaderStream.str();
			}
			catch (ifstream::failure e)
			{
				cout << "Error : Shader file not succesfully read." << endl;
			}

			const char* vShaderCode = vertexCode.c_str();
			const char* fShaderCode = fragmentCode.c_str();

			unsigned int vertex;
			unsigned int fragment;
			int success;
			char infoLog[1024];

			vertex = glCreateShader(GL_VERTEX_SHADER);
			glShaderSource(vertex, 1, &vShaderCode, NULL);
			glCompileShader(vertex);
			glGetShaderiv(vertex, GL_COMPILE_STATUS, &success);
			if (!success) 
			{
				glGetShaderInfoLog(vertex, 1024, NULL, infoLog);
				cout << "Error : VertexShader error: " << infoLog << endl;
			}

			fragment = glCreateShader(GL_FRAGMENT_SHADER);
			glShaderSource(fragment, 1, &fShaderCode, NULL);
			glCompileShader(fragment);
			glGetShaderiv(fragment, GL_COMPILE_STATUS, &success);
			if (!success) 
			{
				glGetShaderInfoLog(fragment, 1024, NULL, infoLog);
				cout << "Error : FragmentShader error: " << infoLog << endl;
			}

			ID = glCreateProgram();
			glAttachShader(ID, vertex);
			glAttachShader(ID, fragment);
			glLinkProgram(ID);
			glGetProgramiv(ID, GL_LINK_STATUS, &success);
			if (!success) 
			{
				glGetProgramInfoLog(ID, 1024, NULL, infoLog);
				cout << "Error : Program error: " << infoLog << endl;
			}

			glDeleteShader(vertex);
			glDeleteShader(fragment);
		}

		void Use()//激活程序
		{
			glUseProgram(ID);
		}

		//uniform工具函数
		void SetUniformInt(const string& name, int value) const
		{
			int location = glGetUniformLocation(ID, name.c_str());
			glUniform1i(location, value);
		}

		void SetUniformFloat(const string& name, float value) const
		{
			int location = glGetUniformLocation(ID, name.c_str());
			glUniform1f(location, value);
		}

		void SetUniformBool(const string& name, bool value) const
		{
			int location = glGetUniformLocation(ID, name.c_str());
			glUniform1i(location, value);
		}

		void SetUniformColor(const string& name, float r, float g, float b) const
		{
			int location = glGetUniformLocation(ID, name.c_str());
			glUniform4f(location, r, g, b, 1);
		}
};

#endif // !SHADER_H