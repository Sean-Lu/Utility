--通过标准SQL语句搭建SQL Server数据库，示例如下：

--指向当前要使用的数据库
use master
go
--判断当前数据库是否存在
if exists (select * from sysdatabases where name='StudentManageDB')
drop database StudentManageDB--删除数据库
go
--创建数据库
create database StudentManageDB
on primary
(
	--数据库文件的逻辑名
    name='StudentManageDB_data',
    --数据库物理文件名（绝对路径）
    filename='D:\DB\StudentManageDB_data.mdf',
    --数据库文件初始大小
    size=10MB,
    --数据文件增长量
    filegrowth=1MB
)
--创建日志文件
log on
(
    name='StudentManageDB_log',
    filename='D:\DB\StudentManageDB_log.ldf',
    size=2MB,
    filegrowth=1MB
)
go
--创建学员信息数据表
use StudentManageDB
go
if exists (select * from sysobjects where name='Students')
drop table Students
go
create table Students
(
    StudentId int identity(100000,1) ,
    StudentName varchar(20) not null,
    Gender char(2)  not null,
    Birthday smalldatetime  not null,
    StudentIdNo numeric(18,0) not null,--身份证号
    CardNo  varchar(20) not null,--考勤卡号
    Age int not null,
    PhoneNumber varchar(50),
    StudentAddress varchar(500),
    ClassId int not null  --班级外键
)
go
--创建班级表
if exists(select * from sysobjects where name='StudentClass')
drop table StudentClass
go
create table StudentClass
(
	ClassId int primary key,
    ClassName varchar(20) not null
)
go
--创建成绩表
if exists(select * from sysobjects where name='ScoreList')
drop table ScoreList
go
create table ScoreList
(
    Id int identity(1,1) primary key,
    StudentId int not null,
    CSharp int null,
    SQLServerDB int null,
    UpdateTime smalldatetime not null
)
go
--创建考勤表
if exists(select * from sysobjects where name='Attendance')
drop table Attendance
create table Attendance
(
	Id int identity(100000,1) primary key,--标识列
    CardNo varchar(20) not null,--学员卡号
    DTime smalldatetime not null --打卡时间
)
go
--创建管理员用户表
if exists(select * from sysobjects where name='Admins')
drop table Admins
create table Admins
(
	LoginId int identity(1000,1) primary key,
    LoginPwd varchar(20) not null,
    AdminName varchar(20) not null
)
go
--创建数据表的各种约束
use StudentManageDB
go
--创建“主键”约束primary key
if exists(select * from sysobjects where name='pk_StudentId')
alter table Students drop constraint pk_StudentId

alter table Students
add constraint pk_StudentId primary key (StudentId)

--创建检查约束check
if exists(select * from sysobjects where name='ck_Age')
alter table Students drop constraint ck_Age
alter table Students
add constraint ck_Age check (Age between 18 and 35) 

--创建唯一约束unique
if exists(select * from sysobjects where name='uq_StudentIdNo')
alter table Students drop constraint uq_StudentIdNo
alter table Students
add constraint uq_StudentIdNo unique (StudentIdNo)

if exists(select * from sysobjects where name='uq_CardNo')
alter table Students drop constraint uq_CardNo
alter table Students
add constraint uq_CardNo unique (CardNo)

--创建身份证的长度检查约束
if exists(select * from sysobjects where name='ck_StudentIdNo')
alter table Students drop constraint ck_StudentIdNo
alter table Students
add constraint ck_StudentIdNo check (len(StudentIdNo)=18)

--创建默认约束 
if exists(select * from sysobjects where name='df_StudentAddress')
alter table Students drop constraint df_StudentAddress
alter table Students 
add constraint df_StudentAddress default ('地址不详' ) for StudentAddress

if exists(select * from sysobjects where name='df_UpdateTime')
alter table ScoreList drop constraint df_UpdateTime
alter table ScoreList 
add constraint df_UpdateTime default (getdate() ) for UpdateTime

if exists(select * from sysobjects where name='df_DTime')
alter table Attendance drop constraint df_DTime
alter table Attendance 
add constraint df_DTime default (getdate() ) for DTime

--创建外键约束
if exists(select * from sysobjects where name='fk_classId')
alter table Students drop constraint fk_classId
alter table Students
add constraint fk_classId foreign key (ClassId) references StudentClass(ClassId)

if exists(select * from sysobjects where name='fk_StudentId')
alter table ScoreList drop constraint fk_StudentId
alter table ScoreList
add constraint fk_StudentId foreign key(StudentId) references Students(StudentId)


-------------------------------------------插入数据--------------------------------------
use StudentManageDB
go


--插入管理员信息
insert into Admins (LoginPwd,AdminName) values(123456,'王晓军')
insert into Admins (LoginPwd,AdminName) values(123456,'张明丽')


select * from Admins





