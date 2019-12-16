const router = require('express').Router();
const mongojs = require('mongojs');
const db = mongojs('mongodb://admintfg:password1234@ds259377.mlab.com:59377/videogame-sd-db', ['users']);
const jwt = require('jsonwebtoken');
const SECRET_KEY = 'key1234';
const sha256 = require('js-sha256').sha256;

const express = require('express');

const app = express();
const server = require('http').Server(app);
const io = require('socket.io')(server);


//SOCKET.IO
io.on('connection', function (socket) {
    console.log('socket');
    let previousId;
    const safeJoin = currentId => {
        socket.leave(previousId);
        socket.join(currentId);
        previousId = currentId;
    };

    socket.on("getDoc", docId => {
        safeJoin(docId);
        socket.emit("document", db[docId]);
    });

});


router.post('/login', (req, res, next) => {
    const userData = {
        username: req.body.username,
        password: req.body.password
    }

    db.users.findOne({ username: userData.username }, (err, user) => {
        if (err) return res.status(500).send('Server error!');

        if (!user) {
            res.status(404).send({ message: 'User not found' });
        } else {
            let hash = sha256(userData.password);
            const resultPassword = user.password == hash;
            if (resultPassword) {
                const expiresIn = 24 * 60 * 60;
                const accessToken = jwt.sign({ id: user.id }, SECRET_KEY, { expiresIn: expiresIn });
                console.log(user);
                const dataUser = {
                    username: user.username,
                    email: user.email,
                    accessToken: accessToken,
                    expiresIn: expiresIn
                }
                res.send({ dataUser });
            } else {
                //password wrong
                res.status(404).send({ message: 'Incorrect password' });
            }

        }
    });
});


//GET all
router.get('/users', (req, res, next) => {
    // res.send('API Here');
    db.users.find((err, users) => {
        if (err) return next(err);
        res.json(users);
    });

});

//GET por id
router.get('/users/:id', (req, res, next) => {
    db.users.findOne({ _id: mongojs.ObjectId(req.params.id) }, (err, user) => {
        if (err) return next(err);
        res.json(user);
    });
});



//PRUEBA POST 
router.post('/users/prueba', (req, res, next) => {

    console.log(req.body);

});

//POST
router.post('/users', (req, res, next) => {
    const user = req.body;
    //validaciones   || user.total_score!= null
    console.log(user);

    if (!user.name || !user.games || !user.username || !user.email || !user.password || !user.init_date || !user.birthdate || user.total_score == null) {
        res.status(400).json({
            error: 'Bad data'
        });
    } else {
        db.users.save(user, (err, user) => {
            if (err) return next(err);
            res.json(user);
        });
    }
});


//DELETE
router.delete('/users/:id', (req, res, next) => {
    db.users.remove({ _id: mongojs.ObjectId(req.params.id) }, (err, result) => {
        if (err) return next(err);
        res.json(result);
    });
});


router.put('/users/:id', (req, res, next) => {
    const data = req.body;
    const updateData = {};

    //validamos los datos recibidos por el cliente

    if (data.name) {
        updateData.isDone = data.name;
    }

    if (data.username) {
        updateData.username = data.username;
    }

    if (data.email) {
        updateData.email = data.email;
    }
    if (data.password) {
        updateData.password = data.password;
    }
    if (data.birthdate) {
        updateData.birthdate = data.birthdate;
    }
    if (data.init_date) {
        updateData.init_date = data.init_date;
    }
    if (data.games) {
        updateData.games = data.games;
    }
    if (data.games.planificacion) {
        updateData.games.planificacion = data.games.planificacion;
    }
    if (data.games.memoria) {
        updateData.games.memoria = data.games.memoria;
    }
    if (data.total_score) {
        updateData.total_score = data.total_score;
    }
    if (!updateData) {
        res.status(400).json({
            error: 'Bad Request'
        });
    } else {
        db.users.update({ _id: mongojs.ObjectId(req.params.id) }, updateData, (err, data) => {
            if (err) return next(err);
            res.json(data);
        })
    }


});

module.exports = router;
