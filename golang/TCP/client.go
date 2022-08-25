package main

import (
	"fmt"
	"net"
	"os"
	"sync"
)

func client(id int) {
	serverAddr := "localhost:10000"
	client, err := net.ResolveTCPAddr("tcp", serverAddr)
	if err != nil {
		os.Exit(1)
	}

	conn, err := net.DialTCP("tcp", nil, client)
	if err != nil {
		os.Exit(1)
	}

	loops := 0
	for loops < 5 {
		msg := fmt.Sprintf("Hello %d from %d", loops, id)
		_, err = conn.Write([]byte(msg))
		if err != nil {
			os.Exit(1)
		}

		reply := make([]byte, 1024)

		_, err = conn.Read(reply)
		if err != nil {
			os.Exit(1)
		}

		println("reply from server = ", string(reply))

		loops += 1
	}

	conn.Close()
}

func main() {
    var wg sync.WaitGroup

    for i:= 1; i <= 20; i++ {
        wg.Add(1)

        i := i

        go func() {
            defer wg.Done()
            client(i)
        }()
    }

    wg.Wait()
}
