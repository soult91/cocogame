
const express = require('express');
const app = express();
const http = require('http').createServer(app);
const io = require('socket.io')(http);  

app.get('/hello', function(req, res){
    res.status(200).send('hello world');
});

io.on('connection', function(){
    console.log('co');
});

app.listen(3000, function(){
    console.log('server connected');
})