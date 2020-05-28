# How to build tgpbot docker on local
# (should run the commands in powershell with administator)

### 1. Install docker registry on local if not exist
> Run command:
```
docker run -d -p 5000:5000 -v /home/user1/storage:/var/lib/registry --name registry registry:2
```

### 2. Build docker image with docker file
> Assume image name is: localhost:5000/tgpbot:development 
(should the same in docker-compose.yml)  
> Under tgpbot project, run command:
```
docker build -f TGPBot/Dockerfile -t localhost:5000/tgpbot:development --rm --no-cache TGPBot/.
```

### 3. Pull image to local docker registry
> Run command:
```
docker push localhost:5000/tgpbot:development
```

### 4. Pull image and up container
> Move to the folder that docker-compose.yml is located (TGPBot project folder)  
> Pull image:
```
docker-compose -f docker-compose.yml -f docker-compose.development.yml -p development pull botservice
```

> up container:
```
docker-compose -f docker-compose.yml -f docker-compose.development.yml -p development up -d --no-deps --build botservice
```

Note:
> the format of docker-compose command would be:
```
docker-compose -f docker-compose.yml -f docker-compose.$env.yml -p $env {commands}
```
> the $env parameter will be different between the enviromnents (deveployment / staging / production),  
> and the docker-compose.yml will be combined against the enviromnent.


# TGPBot Announce Guid
### 1.Add TGP announcement  TGP 平台公告 to skype group

### 2.Add this group to announce list, and bot will response [Added TESTA]
> @TGP announcement  TGP 平台公告  add TESTA


# Test tgpbot on local
> 1. Download Bot Framework Emulator V4  
> 2. Run/Debug tgpbot on local  
> 3. Link to: http://localhost:3978/api/messages with credential (in appsetting.json)