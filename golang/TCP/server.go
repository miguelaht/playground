package main

import (
    "fmt"
    "io"
    "net"
    "os"
)

func main() {
    ln, err := net.Listen("tcp", ":10000")
    if err != nil {
        fmt.Println("listen: ", err.Error())
        os.Exit(1)
    }
    fmt.Println("listening on port 10000")
    for {
        conn, err := ln.Accept()
        if err != nil {
            fmt.Println("accept: ", err.Error())
            os.Exit(1)
        }
        fmt.Println("connection from ", conn.RemoteAddr())
        go handle(conn)
    }
}

func handle(conn net.Conn) {
    buf := make([]byte, 1024)

    for {
        readlen, err := conn.Read(buf)
        if err != nil {
            if err == io.EOF {
                fmt.Println("eof from ", conn.RemoteAddr())
            } else {
                fmt.Println("read: ", err.Error())
            }
            conn.Close()
            return
        }

        _, err = conn.Write(buf[:readlen])
        if err != nil {
            fmt.Println("write: ", err.Error())
            return
        }
    }
}
