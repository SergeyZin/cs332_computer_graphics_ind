#include <Windows.h>
#include <GL\glew.h>
#include <GL\freeglut.h>
#include <iostream>

using namespace std;

double rotateX = 0;
double rotateY = 0;
double rotateZ = 0;
double Angle = 0;


void renderWireCube()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glLoadIdentity();
	
	glRotatef(rotateX, 1.0, 0.0, 0.0);
	glRotatef(rotateY, 0.0, 1.0, 0.0);
	glRotatef(rotateZ, 0.0, 0.0, 1.0);
	
	glutWireCube(1);
	
	glFlush();
	glutSwapBuffers();
}


void renderWireTetrahedron()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glLoadIdentity();
	glutWireTetrahedron();
	glutSwapBuffers();
}

void renderWireOctahedron()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glLoadIdentity();

	glRotatef(rotateX, 1.0, 0.0, 0.0);
	glRotatef(rotateY, 0.0, 1.0, 0.0);
	glRotatef(rotateZ, 0.0, 0.0, 1.0);

	glutWireOctahedron();

	glFlush();
	glutSwapBuffers();
}

void renderColoredTriangle()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glLoadIdentity();
	glRotatef(Angle, rotateX, rotateY, rotateZ);
	
	glBegin(GL_TRIANGLES);
	glColor3ub(rand() % 255, 0.0, 0.0);
	glVertex2f(-0.5f, -0.5f);
	glColor3ub(0.0, rand() % 255, 0.0);
	glVertex2f(-0.5f, 0.5f);
	glColor3ub(0.0, 0.0, rand() % 255);
	glVertex2f(0.5f, 0.5f);
	glEnd();

	glFlush();
	glutSwapBuffers();
}

void renderColoredRectangle()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glLoadIdentity();
	glRotatef(Angle, rotateX, rotateY, rotateZ);
	glBegin(GL_TRIANGLES);
	glColor3ub(rand() % 255, 0.0, 0.0);
	glVertex2f(-0.5f, -0.5f);
	glColor3ub(0.0, rand() % 255, 0.0);
	glVertex2f(-0.5f, 0.5f);
	glColor3ub(0.0, 0.0, rand() % 255);
	glVertex2f(0.5f, 0.5f);
	glColor3ub(rand() % 255, rand() % 255, rand() % 255);
	glVertex2f(0.5f, -0.5f);
	glEnd();

	glFlush();
	glutSwapBuffers();
}


void mouseClick(int button, int state, int x, int y) {
	if (button == GLUT_LEFT_BUTTON)
		renderColoredTriangle();
}


void Reshape(int w, int h)
{
	glViewport(0, 0, w, h);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(65.0f, w / h, 1.0f, 1000.0f);
	glMatrixMode(GL_MODELVIEW);
}

void specialKeys(int key, int x, int y) {
	switch (key) {
	case GLUT_KEY_UP: rotateX += 5; break;
	case GLUT_KEY_DOWN: rotateX -= 5; break;

	case GLUT_KEY_RIGHT: rotateY += 5; break;
	case GLUT_KEY_LEFT: rotateY -= 5; break;

	case GLUT_KEY_PAGE_UP: rotateZ += 5; break;
	case GLUT_KEY_PAGE_DOWN: rotateZ -= 5; break;
	}

	glutPostRedisplay();
}

int main(int argc, char* argv[]) {

	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA | GLUT_DEPTH);
	glutInitWindowSize(600, 600);
	glutCreateWindow("Assignment 10");

	glutDisplayFunc(renderWireOctahedron);
	glutSpecialFunc(specialKeys);
	glutMouseFunc(mouseClick);

	GLenum err = glewInit();
	if (GLEW_OK != err) {
		fprintf(stderr, "GLEW error");
		return 1;
	}

	glutMainLoop();

	return 0;
}