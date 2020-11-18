cd space-udp-messages
cd ..
flatc.exe --csharp -I .\space-udp-messages\schema\serverclientmessages\ -I .\space-udp-messages\schema\clientservermessages\ --filename-suffix '""' .\space-udp-messages\UdpMessages.fbs space-udp-messages/schema/serverclientmessages/*.fbs space-udp-messages/schema/clientservermessages/*.fbs space-udp-messages/schema/utilities/*.fbs