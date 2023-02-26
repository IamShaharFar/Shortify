let genbtn = document.querySelector('#gen');
let shorturl = document.querySelector('#shorturl');
genbtn.addEventListener("click", generate)
let CopyBtn = document.querySelector('#Copyurl');
CopyBtn.addEventListener("click", CopyToClipBoard);
let CleanBtn = document.querySelector('#Clean');
CleanBtn.addEventListener("click", ClearUrl);
let valid = 0;
let openbtn = document.querySelector("#open");

let doshortapi = 'https://localhost:7207/api/doshort?fullurl=';

async function generate() {
    let fullurl = document.getElementById("fullurl").value;
    let requrl = doshortapi + fullurl;
    if (isValidHttpUrl(fullurl)) {
        let res = await fetch(requrl, {
            method: "POST",
            body: "{}",
            headers: { "Content-type": "application/text;" }
        })
            .then(response => response.text())
            .then(data => { shorturl.value = data; console.log(data); });
        valid = 1;
        openbtn.addEventListener("click", open)
    }
    else {
        alert("please enter valid url");
    }
}

function CopyToClipBoard(shorturl) {
    let shorturlinput = document.querySelector('#shorturl');
    navigator.clipboard.writeText(shorturlinput.value);
    alertclip();
}

function alertclip() {
    let alal = document.querySelector("#alal")
    alal.style.display = "block"
    
    setTimeout(function () {
        alal.style.display = "none";
    }, 2000);
}

function ClearUrl() {
    document.getElementById("fullurl").value = "";
    document.getElementById("shorturl").value = "";
    valid = 0;
    openbtn.removeEventListener("click",open)
}

function isValidHttpUrl(fullurl) {
    let url;
    try {
        url = new URL(fullurl);
    } catch (_) {
        return false;
    }
    return url.protocol === "http:" || url.protocol === "https:";
}

function open() {
    let s = document.querySelector("#shorturl").value;
    window.location.assign(s);
}