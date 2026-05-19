package com.example.tcpserverexample;

import com.example.tcpserverexample.access.CommandAllocator;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class Application {

    public static void main(String[] args) {
        CommandAllocator.initializeListen(9000);
        SpringApplication.run(Application.class, args);
    }

}
