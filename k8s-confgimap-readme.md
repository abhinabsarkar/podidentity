# Kubernetes ConfigMap with Docker Desktop

## Test the ConfigMap locally on Docker Desktop
```bash
# Create a namespace
kubectl create namespace abs
# Apply the yaml file which will create the resources in k8s cluster
kubectl apply -f akvaspnetapp.yaml -n abs
# View the details of the configmap created
kubectl describe configmap/akvaspnetapp -n abs
# Get the status of all kubernetes objects created under namespace abs
kubectl get all -n abs -o wide
NAME                                READY   STATUS    RESTARTS   AGE   IP           NODE             NOMINATED NODE   READINESS GATES
pod/akvaspnetapp-5c7677c8f6-rzb8v   1/1     Running   0          13m   10.1.1.212   docker-desktop   <none>           <none>

NAME                   TYPE           CLUSTER-IP     EXTERNAL-IP   PORT(S)        AGE   SELECTOR
service/akvaspnetapp   LoadBalancer   10.101.3.188   localhost     80:30878/TCP   13m   app=akvaspnetapp

NAME                           READY   UP-TO-DATE   AVAILABLE   AGE   CONTAINERS     IMAGES                              SELECTOR
deployment.apps/akvaspnetapp   1/1     1            1           13m   akvaspnetapp   abhinabsarkar/abs-akvaspnetapp:v1   app=akvaspnetapp

NAME                                      DESIRED   CURRENT   READY   AGE   CONTAINERS     IMAGES                              SELECTOR
replicaset.apps/akvaspnetapp-5c7677c8f6   1         1         1       13m   akvaspnetapp   abhinabsarkar/abs-akvaspnetapp:v1   app=akvaspnetapp,pod-template-hash=5c7677c8f6
```

Validate by running the below curl command
```bash
curl http://localhost/keyvault
```

If the container doesn't spin up, look into the container logs
```bash
docker logs <container_id>
```

### Next Steps
* Run this on AKS

## References
* [Config Maps in dotnet core](https://medium.com/@fbeltrao/automatically-reload-configuration-changes-based-on-kubernetes-config-maps-in-a-net-d956f8c8399a)