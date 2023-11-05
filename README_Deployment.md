# Deployment Instructions for ASP.NET Core Application using Docker

## Prerequisites
- [Docker](https://docs.docker.com/get-docker/) installed on your host machine.

## Step 1: Build the Docker Image

open your cmd or Terminal window and go to the project folde
Enter the command :
docker build -t vehicle-status-tracker .

This command builds the Docker image from the provided Dockerfile and tags it as "vehicle-status-tracker". You can change the tag to your preferred image name.

## Step 2: Run the Docker Container

docker run -d -p 8080:80 --name vehicle-status-tracker-container vehicle-status-tracker

This command runs the Docker container in detached mode, maps port 8080 on the host to port 80 in the container, and gives it the name "vehicle-status-tracker-container".

## Step 3: Access Your Application
You can access the application by opening a web browser and navigating to http://localhost:8080. If the application is hosted on a different port, replace 8080 with the appropriate port number.

## Step 4: Stopping and Cleaning Up

To stop the container:
docker stop vehicle-status-tracker-container

To remove the container:
docker rm vehicle-status-tracker-container
