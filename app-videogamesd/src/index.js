const express = require('express');
const path = require('path');
const cors = require('cors');

const app = express();
//  const socketRoutes = require('./routes/socket');
const usersRoutes = require('./routes/users');

const server = require('http').Server(app); 
const io = require('socket.io')(server);

// middlewares
app.use(cors());
app.use(express.json());
app.use(express.urlencoded({extended: false}));

// settings
// app.set('views', path.join(__dirname, 'views'));
// app.engine('html', require('ejs').renderFile);
// app.set('view engine', 'ejs');

app.set('port', process.env.PORT || 3000);

// routes
// app.use('/socket', socketgyRoutes);
app.use('/api', usersRoutes);

// static files
app.use(express.static(path.join(__dirname, 'dist')));

io.on('connection', function(socket){
    console.log('socket');
});

// start the server
app.listen(app.get('port'), () => {
    console.log('server on port 3000');
}); 
