create database User;
create table User
(
    id serial primary key,
    first_name varchar not null,
    last_name varcahr not null,
    age int not null,
);