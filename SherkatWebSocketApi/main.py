import asyncio
import websockets

from fastapi import FastAPI, File, UploadFile, Depends, WebSocket
from fastapi.responses import HTMLResponse

app = FastAPI()
uploadFileHtml = """
<!DOCTYPE html>
<html>
<head>
    <title>Upload a file</title>
</head>
<body>
    <h1>Upload a file</h1>
    <form id="upload-form">
        <label for="file">Choose a file:</label>
        <input type="file" name="file" id="file"><br>
        <button type="submit">Upload</button>
    </form>

    <script>
        const form = document.getElementById("upload-form");
        form.addEventListener("submit", async (event) => {
            event.preventDefault();

            const formData = new FormData(form);
            try {
                const response = await fetch("/upload", {
                    method: "POST",
                    body: formData
                });
                const data = await response.json();
                alert("File uploaded: " + data.filename);
            } catch (error) {
                console.error(error);
            }
        });
    </script>
</body>
</html>
"""
html = """
<!DOCTYPE html>
<html>
    <head>
        <title>Chat</title>
    </head>
    <body>
        <h1>WebSocket Chat</h1>
        <form action="" onsubmit="sendMessage(event)">
            <input type="text" id="messageText" autocomplete="off"/>
            <button>Send</button>
        </form>
        <ul id='messages'>
        </ul>
        <script>
            var ws = new WebSocket("ws://localhost:8000/ws");
            ws.onmessage = function(event) {
                var messages = document.getElementById('messages')
                var message = document.createElement('li')
                var content = document.createTextNode(event.data)
                message.appendChild(content)
                messages.appendChild(message)
            };
            function sendMessage(event) {
                var input = document.getElementById("messageText")
                ws.send(input.value)
                input.value = ''
                event.preventDefault()
            }
        </script>
    </body>
</html>
"""
connected_clients = set()


async def send_message(websocket: websockets.WebSocketClientProtocol, message):
    await websocket.send(message)


@app.get("/")
async def get():
    return HTMLResponse(html)


@app.get("/upload-file")
async def upload_file_form():
    return HTMLResponse(uploadFileHtml)


@app.post("/upload")
async def upload_file(file: UploadFile = File(...)):
    # Save uploaded file here

    # Send WebSocket message to all connected regular clients
    message = file.filename
    print(message)
    print(connected_clients)
    for client in connected_clients:
        await send_message(client, message)

    return {"filename": file.filename}


@app.websocket("/ws")
async def websocket_endpoint(websocket: WebSocket):
    await websocket.accept()
    connected_clients.add(websocket)
    print(connected_clients)
    # Handle incoming messages
    while True:
        data = await websocket.receive_text()
        await websocket.send_text(f"Message text was: {data}")

    # Remove WebSocket client from set of connected clients
    # connected_clients.remove(websocket)
