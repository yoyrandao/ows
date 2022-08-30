cmake_minimum_required(VERSION 3.13)
project(ows-window-api)

set(CMAKE_CXX_STANDARD 14)
set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -DTWINDOWSLIBRARY_EXPORTS")

add_library(ows-window-api MODULE twindows.cpp)