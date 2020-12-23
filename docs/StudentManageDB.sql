--ͨ����׼SQL���SQL Server���ݿ⣬ʾ�����£�

--ָ��ǰҪʹ�õ����ݿ�
use master
go
--�жϵ�ǰ���ݿ��Ƿ����
if exists (select * from sysdatabases where name='StudentManageDB')
drop database StudentManageDB--ɾ�����ݿ�
go
--�������ݿ�
create database StudentManageDB
on primary
(
	--���ݿ��ļ����߼���
    name='StudentManageDB_data',
    --���ݿ������ļ���������·����
    filename='D:\DB\StudentManageDB_data.mdf',
    --���ݿ��ļ���ʼ��С
    size=10MB,
    --�����ļ�������
    filegrowth=1MB
)
--������־�ļ�
log on
(
    name='StudentManageDB_log',
    filename='D:\DB\StudentManageDB_log.ldf',
    size=2MB,
    filegrowth=1MB
)
go
--����ѧԱ��Ϣ���ݱ�
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
    StudentIdNo numeric(18,0) not null,--���֤��
    CardNo  varchar(20) not null,--���ڿ���
    Age int not null,
    PhoneNumber varchar(50),
    StudentAddress varchar(500),
    ClassId int not null  --�༶���
)
go
--�����༶��
if exists(select * from sysobjects where name='StudentClass')
drop table StudentClass
go
create table StudentClass
(
	ClassId int primary key,
    ClassName varchar(20) not null
)
go
--�����ɼ���
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
--�������ڱ�
if exists(select * from sysobjects where name='Attendance')
drop table Attendance
create table Attendance
(
	Id int identity(100000,1) primary key,--��ʶ��
    CardNo varchar(20) not null,--ѧԱ����
    DTime smalldatetime not null --��ʱ��
)
go
--��������Ա�û���
if exists(select * from sysobjects where name='Admins')
drop table Admins
create table Admins
(
	LoginId int identity(1000,1) primary key,
    LoginPwd varchar(20) not null,
    AdminName varchar(20) not null
)
go
--�������ݱ�ĸ���Լ��
use StudentManageDB
go
--������������Լ��primary key
if exists(select * from sysobjects where name='pk_StudentId')
alter table Students drop constraint pk_StudentId

alter table Students
add constraint pk_StudentId primary key (StudentId)

--�������Լ��check
if exists(select * from sysobjects where name='ck_Age')
alter table Students drop constraint ck_Age
alter table Students
add constraint ck_Age check (Age between 18 and 35) 

--����ΨһԼ��unique
if exists(select * from sysobjects where name='uq_StudentIdNo')
alter table Students drop constraint uq_StudentIdNo
alter table Students
add constraint uq_StudentIdNo unique (StudentIdNo)

if exists(select * from sysobjects where name='uq_CardNo')
alter table Students drop constraint uq_CardNo
alter table Students
add constraint uq_CardNo unique (CardNo)

--�������֤�ĳ��ȼ��Լ��
if exists(select * from sysobjects where name='ck_StudentIdNo')
alter table Students drop constraint ck_StudentIdNo
alter table Students
add constraint ck_StudentIdNo check (len(StudentIdNo)=18)

--����Ĭ��Լ�� 
if exists(select * from sysobjects where name='df_StudentAddress')
alter table Students drop constraint df_StudentAddress
alter table Students 
add constraint df_StudentAddress default ('��ַ����' ) for StudentAddress

if exists(select * from sysobjects where name='df_UpdateTime')
alter table ScoreList drop constraint df_UpdateTime
alter table ScoreList 
add constraint df_UpdateTime default (getdate() ) for UpdateTime

if exists(select * from sysobjects where name='df_DTime')
alter table Attendance drop constraint df_DTime
alter table Attendance 
add constraint df_DTime default (getdate() ) for DTime

--�������Լ��
if exists(select * from sysobjects where name='fk_classId')
alter table Students drop constraint fk_classId
alter table Students
add constraint fk_classId foreign key (ClassId) references StudentClass(ClassId)

if exists(select * from sysobjects where name='fk_StudentId')
alter table ScoreList drop constraint fk_StudentId
alter table ScoreList
add constraint fk_StudentId foreign key(StudentId) references Students(StudentId)


-------------------------------------------��������--------------------------------------
use StudentManageDB
go


--�������Ա��Ϣ
insert into Admins (LoginPwd,AdminName) values(123456,'������')
insert into Admins (LoginPwd,AdminName) values(123456,'������')


select * from Admins





