@echo off

IF NOT EXIST Data mkdir Data

IF NOT EXIST Data\mongodb mkdir Data\mongodb

call Tools\mongoDB\mongod --port 27018 --dbpath Data\mongodb\