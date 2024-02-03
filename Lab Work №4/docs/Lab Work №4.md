# Лабораторная работа №4
**Тема:** Проектирование REST API

**Цель работы:** Получить опыт проектирования программного интерфейса.

Код запросов представлен по расположению: "./Lab Work №4/src/TimeTracker/Controllers"
## Документация по API

### GetInitData
**метод:** GET

**описание:** получение имени текущей задачи, проекта и время начала выполнения

**параметры запроса:** 
chatId: long (идентификатор чата с пользователем в телеграмме)

**ответ:**
GetInfoEntity
- FIO: string | null (ФИО, заданное пользователем в телеграмме)
- TaskName: string (имя задачи)
- ProjectName: string (имя проекта)
- StartTime: DateTime | null (дата и время начала выполнения)


### GetProjectList
**метод:** GET

**описание:** получение списка проектов для текущего пользователя

**параметры запроса:** 
chatId: long (идентификатор чата с пользователем в телеграмме)

**ответ:**
Список DBProject
- Id: string (уникальный идентификатор проекта)
- Name: string (имя проекта)
- Description: string | null (описание проекта)


### GetTaskList
**метод:** GET

**описание:** получение списка задач для текущего пользователя

**параметры запроса:** 
chatId: long (идентификатор чата с пользователем в телеграмме)

**ответ:**
Список DBTask
- Id: string (уникальный идентификатор задачи)
- СhatId: long (идентификатор чата с пользователем в телеграмме)
- TaskName: string (имя задачи)
- TaskDescription: string (описание задачи)
- Project: DBProject (проект, к которому прикреплена задача)
	- Id: string (уникальный идентификатор проекта)
	- Name: string (имя проекта)
	- Description: string | null (описание проекта)
- StartTime: DateTime | null (время запуска задачи)
- EndTime: DateTime | null (время остановки задачи)
- StartPosition: DBGPS | null (местоположение запуска задачи)
	- Id: string (уникальный идентификатор GPS)
	- Latitude: double (широта)
	- Longitude: double (долгота)
- EndPosition: DBGPS | null (местоположение остановки задачи)
	- Id: string (уникальный идентификатор GPS)
	- Latitude: double (широта)
	- Longitude: double (долгота)

### PostNewTask
**метод:** POST

**описание:** создание новой задачи для текущего пользователя

**параметры запроса:** 
TaskEntity (объект, содержащий информацию о задаче)
- СhatId: long (идентификатор чата с пользователем в телеграмме)
- TaskName: string (имя задачи)
- TaskDescription: string (описание задачи)
- ProjectName: string (имя проекта)
- ProjectDescription: string | null (описание проекта)
- StartTime: DateTime | null (время запуска задачи)
- EndTime: DateTime | null (время остановки задачи)
- StartLatitude: double (широта)
- StartLongitude: double (долгота)
- EndLatitude: double (широта)
- EndLongitude: double (долгота)

**ответ:**
DBTask
- Id: string (уникальный идентификатор задачи)
- СhatId: long (идентификатор чата с пользователем в телеграмме)
- TaskName: string (имя задачи)
- TaskDescription: string (описание задачи)
- Project: DBProject (проект, к которому прикреплена задача)
	- Id: string (уникальный идентификатор проекта)
	- Name: string (имя проекта)
	- Description: string | null (описание проекта)
- StartTime: DateTime | null (время запуска задачи)
- EndTime: DateTime | null (время остановки задачи)
- StartPosition: DBGPS | null (местоположение запуска задачи)
	- Id: string (уникальный идентификатор GPS)
	- Latitude: double (широта)
	- Longitude: double (долгота)
- EndPosition: DBGPS | null (местоположение остановки задачи)
	- Id: string (уникальный идентификатор GPS)
	- Latitude: double (широта)
	- Longitude: double (долгота)

### PostNewProject
**метод:** POST

**описание:** создание нового проекта

**параметры запроса:** 
ProjectEntity (объект, содержащий информацию о проекте)
- ProjectName: string (имя проекта)
- ProjectDescription: string | null (описание проекта)

**ответ:**
Список DBProject
- Id: string (уникальный идентификатор проекта)
- Name: string (имя проекта)
- Description: string | null (описание проекта)

### DeleteTask
**метод:** DELETE

**описание:** удаление задачи

**параметры запроса:** 
TaskEntity (объект, содержащий информацию о задаче)
- СhatId: long (идентификатор чата с пользователем в телеграмме)
- TaskName: string (имя задачи)
- TaskDescription: string (описание задачи)
- ProjectName: string (имя проекта)
- ProjectDescription: string | null (описание проекта)
- StartTime: DateTime | null (время запуска задачи)
- EndTime: DateTime | null (время остановки задачи)
- StartLatitude: double (широта)
- StartLongitude: double (долгота)
- EndLatitude: double (широта)
- EndLongitude: double (долгота)

**ответ:**
DBTask
- Id: string (уникальный идентификатор задачи)
- СhatId: long (идентификатор чата с пользователем в телеграмме)
- TaskName: string (имя задачи)
- TaskDescription: string (описание задачи)
- Project: DBProject (проект, к которому прикреплена задача)
	- Id: string (уникальный идентификатор проекта)
	- Name: string (имя проекта)
	- Description: string | null (описание проекта)
- StartTime: DateTime | null (время запуска задачи)
- EndTime: DateTime | null (время остановки задачи)
- StartPosition: DBGPS | null (местоположение запуска задачи)
	- Id: string (уникальный идентификатор GPS)
	- Latitude: double (широта)
	- Longitude: double (долгота)
- EndPosition: DBGPS | null (местоположение остановки задачи)
	- Id: string (уникальный идентификатор GPS)
	- Latitude: double (широта)
	- Longitude: double (долгота)

### DeleteProject
**метод:** DELETE

**описание:** удаление проекта

**параметры запроса:** 
ProjectEntity (объект, содержащий информацию о проекте)
- ProjectName: string (имя проекта)
- ProjectDescription: string | null (описание проекта)

**ответ:**
Список DBProject
- Id: string (уникальный идентификатор проекта)
- Name: string (имя проекта)
- Description: string | null (описание проекта)

### PutStartTask
**метод:** PUT
**описание:** запуск задачи, при условии, что она еще не запущена

**параметры запроса:** 
TaskEntity (объект, содержащий информацию о задаче)
- СhatId: long (идентификатор чата с пользователем в телеграмме)
- TaskName: string (имя задачи)
- TaskDescription: string (описание задачи)
- ProjectName: string (имя проекта)
- ProjectDescription: string | null (описание проекта)
- StartTime: DateTime (время запуска задачи)
- EndTime: null (время остановки задачи)
- StartLatitude: double (широта)
- StartLongitude: double (долгота)
- EndLatitude: null (широта)
- EndLongitude: null (долгота)

**ответ:**
DBTask
- Id: string (уникальный идентификатор задачи)
- СhatId: long (идентификатор чата с пользователем в телеграмме)
- TaskName: string (имя задачи)
- TaskDescription: string (описание задачи)
- Project: DBProject (проект, к которому прикреплена задача)
	- Id: string (уникальный идентификатор проекта)
	- Name: string (имя проекта)
	- Description: string | null (описание проекта)
- StartTime: DateTime | null (время запуска задачи)
- EndTime: null (время остановки задачи)
- StartPosition: DBGPS | null (местоположение запуска задачи)
	- Id: string (уникальный идентификатор GPS)
	- Latitude: double (широта)
	- Longitude: double (долгота)
- EndPosition: null (местоположение остановки задачи)

### PutEndTask
**метод:** PUT

**описание:** остановка задачи при условии, что она уже запущена, но еще не остановлена

**параметры запроса:** 
TaskEntity (объект, содержащий информацию о задаче)
- СhatId: long (идентификатор чата с пользователем в телеграмме)
- TaskName: string (имя задачи)
- TaskDescription: string (описание задачи)
- ProjectName: string (имя проекта)
- ProjectDescription: string | null (описание проекта)
- StartTime: DateTime (время запуска задачи)
- EndTime: DateTime (время остановки задачи)
- StartLatitude: double (широта)
- StartLongitude: double (долгота)
- EndLatitude: double (широта)
- EndLongitude: double (долгота)

**ответ:**
DBTask
- Id: string (уникальный идентификатор задачи)
- СhatId: long (идентификатор чата с пользователем в телеграмме)
- TaskName: string (имя задачи)
- TaskDescription: string (описание задачи)
- Project: DBProject (проект, к которому прикреплена задача)
	- Id: string (уникальный идентификатор проекта)
	- Name: string (имя проекта)
	- Description: string | null (описание проекта)
- StartTime: DateTime (время запуска задачи)
- EndTime: DateTime (время остановки задачи)
- StartPosition: DBGPS (местоположение запуска задачи)
	- Id: string (уникальный идентификатор GPS)
	- Latitude: double (широта)
	- Longitude: double (долгота)
- EndPosition: DBGPS (местоположение остановки задачи)
	- Id: string (уникальный идентификатор GPS)
	- Latitude: double (широта)
	- Longitude: double (долгота)

## Тестирование API

### GetInitData
**метод:** GET

**описание:** получение имени текущей задачи, проекта и время начала выполнения

**запрос:**
<p align="left"><img src="../src/img/GetInitData_Request.png" width="750" title="GetInitData request"></p>

**ответ:**
<p align="left"><img src="../src/img/GetInitData_Responce.png" width="750" title="GetInitData responce"></p>

**тесты:**
<p align="left"><img src="../src/img/GetInitData_Tests.png" width="750" title="GetInitData tests"></p>
<p align="left"><img src="../src/img/GetInitData_Results.png" width="750" title="GetInitData test results"></p>

### GetProjectList
**метод:** GET

**описание:** получение списка проектов для текущего пользователя

**запрос:**
<p align="left"><img src="../src/img/GetProjectList_Request.png" width="750" title="GetProjectList request"></p>

**ответ:**
<p align="left"><img src="../src/img/GetProjectList_Responce.png" width="750" title="GetProjectList responce"></p>

**тесты:**
<p align="left"><img src="../src/img/GetProjectList_Tests.png" width="750" title="GetProjectList tests"></p>
<p align="left"><img src="../src/img/GetProjectList_Results.png" width="750" title="GetProjectList test results"></p>

### GetTaskList
**метод:** GET

**описание:** получение списка задач для текущего пользователя

**запрос:**
<p align="left"><img src="../src/img/GetTaskList_Request.png" width="750" title="GetTaskList request"></p>

**ответ:**
<p align="left"><img src="../src/img/GetTaskList_Responce.png" width="750" title="GetTaskList responce"></p>

**тесты:**
<p align="left"><img src="../src/img/GetTaskList_Tests.png" width="750" title="GetTaskList tests"></p>
<p align="left"><img src="../src/img/GetTaskList_Results.png" width="750" title="GetTaskList test results"></p>

### PostNewTask
**метод:** POST

**описание:** создание новой задачи для текущего пользователя

**запрос:**
<p align="left"><img src="../src/img/PostNewTask_Request.png" width="750" title="PostNewTask request"></p>

**ответ:**
<p align="left"><img src="../src/img/PostNewTask_Responce.png" width="750" title="PostNewTask responce"></p>

**тесты:**
<p align="left"><img src="../src/img/PostNewTask_Tests.png" width="750" title="PostNewTask tests"></p>
<p align="left"><img src="../src/img/PostNewTask_Results.png" width="750" title="PostNewTask test results"></p>

### PostNewProject
**метод:** POST

**описание:** создание нового проекта

**запрос:**
<p align="left"><img src="../src/img/PostNewProject_Request.png" width="750" title="PostNewProject request"></p>

**ответ:**
<p align="left"><img src="../src/img/PostNewProject_Responce.png" width="750" title="PostNewProject responce"></p>

**тесты:**
<p align="left"><img src="../src/img/PostNewProject_Tests.png" width="750" title="PostNewProject tests"></p>
<p align="left"><img src="../src/img/PostNewProject_Results.png" width="750" title="PostNewProject test results"></p>

### DeleteTask
**метод:** DELETE

**описание:** удаление задачи

**запрос:**
<p align="left"><img src="../src/img/DeleteTask_Request.png" width="750" title="DeleteTask request"></p>

**ответ:**
<p align="left"><img src="../src/img/DeleteTask_Responce.png" width="750" title="DeleteTask responce"></p>

**тесты:**
<p align="left"><img src="../src/img/DeleteTask_Tests.png" width="750" title="DeleteTask tests"></p>
<p align="left"><img src="../src/img/DeleteTask_Results.png" width="750" title="DeleteTask test results"></p>

### DeleteProject
**метод:** DELETE
**описание:** удаление проекта
**запрос:**
<p align="left"><img src="../src/img/DeleteProject_Request.png" width="750" title="DeleteProject request"></p>

**ответ:**
<p align="left"><img src="../src/img/DeleteProject_Responce.png" width="750" title="DeleteProject responce"></p>

**тесты:**
<p align="left"><img src="../src/img/DeleteProject_Tests.png" width="750" title="DeleteProject tests"></p>
<p align="left"><img src="../src/img/DeleteProject_Results.png" width="750" title="DeleteProject test results"></p>

### PutStartTask
**метод:** PUT

**описание:** запуск задачи, при условии, что она еще не запущена

**запрос:**
<p align="left"><img src="../src/img/PutStartTask_Request.png" width="750" title="PutStartTask request"></p>

**ответ:**
<p align="left"><img src="../src/img/PutStartTask_Responce.png" width="750" title="PutStartTask responce"></p>

**тесты:**
<p align="left"><img src="../src/img/PutStartTask_Tests.png" width="750" title="PutStartTask tests"></p>
<p align="left"><img src="../src/img/PutStartTask_Results.png" width="750" title="PutStartTask test results"></p>

### PutEndTask
**метод:** PUT

**описание:** остановка задачи при условии, что она уже запущена, но еще не остановлена

**запрос:**
<p align="left"><img src="../src/img/PutEndTask_Request.png" width="750" title="PutEndTask request"></p>

**ответ:**
<p align="left"><img src="../src/img/PutEndTask_Responce.png" width="750" title="PutEndTask responce"></p>

**тесты:**
<p align="left"><img src="../src/img/PutEndTask_Tests.png" width="750" title="PutEndTask tests"></p>
<p align="left"><img src="../src/img/PutEndTask_Results.png" width="750" title="PutEndTask test results"></p>
