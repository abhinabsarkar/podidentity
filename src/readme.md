# Docker hub image - abhinabsarkar/abs-akvaspnetapp:v1.1.0
A sample docker app built on Alpine linux 3.1 version using C# dotnet core version 3.1. The sample application accesses an Azure Key Vault under the context of Managed Identity. If the Managed Identity is configured, it returns the secret stored in the Azure Key Vault else it returns a message stating "Cannot access key vault". To run the application locally, run the docker image & browse the application at http://localhost/keyvault

The docker image can be downloaded from docker hub by running the below command 
```bash
docker pull abhinabsarkar/abs-akvaspnetapp:v1.1.0
```
Size of the docker image is 111 MB.

### To build the image, run docker build from the root directory of the application
```bash
# Build the abs image
docker build -t abs-akvaspnetapp:v1.1.0 .
# Run the docker container locally
# The release version runs the container at port 80 although the app is running at port 5000
docker run --name abs-akvaspnetapp-container -d -p 8002:80 abs-akvaspnetapp:v1.1.0
# Check the status of the container
docker ps -a | findstr abs-akvaspnetapp-container
# Test the app
curl http://localhost:8002
# log into the running container 
docker exec -it abs-akvaspnetapp-container /bin/bash
docker exec -it abs-akvaspnetapp-container <command>
# Remove the container
docker rm abs-akvaspnetapp-container -f
# Remove the image
docker rmi abs-akvaspnetapp:v1.1.0
# Push the image to docker hub
docker login
# Tag the local image & map it to the docker repo
docker tag local-image:tagname new-repo:tagname
# eg: docker tag abs-akvaspnetapp:v1.1.0 abhinabsarkar/abs-akvaspnetapp:v1.1.0
# push the tagged image to the docker hub
docker push new-repo:tagname
# eg: docker push abhinabsarkar/abs-akvaspnetapp:v1.1.0
```