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
let gencustombtn = document.querySelector('#customgen');
gencustombtn.addEventListener("click", generatecustom)
let customClean = document.querySelector('#customClean');
customClean.addEventListener("click", ClearUrlCustom)
let customCopyurl = document.querySelector('#customCopyurl');
customCopyurl.addEventListener("click", CopyToClipBoardCustom)

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


async function generatecustom() {
    let customfullurl = document.getElementById("customfullurl").value;
    let customshorturl = document.getElementById("customshorturl").value;
    let req = "https://localhost:7207/api/custom/" + customshorturl;
    let url, status;
    if (isValidHttpUrl(customfullurl)) {
        let res = await fetch(req, {
            method: "POST",
            body: JSON.stringify(customfullurl),
            headers: { "Content-type": "application/json;" }
        })
            .then(resp => { status = resp.status; return resp.text(); })
            .then(data => url = data);
        let customurl = document.querySelector("#customurl");
        let errorcustom = document.querySelector("#errorcustom");
        if (status == 200) { customurl.innerHTML = res; customurl.style.display = "block"; errorcustom.style.display = "none"; }
        else if (status == 409) { errorcustom.style.display = "block"; customurl.style.display = "none"; customurl.innerHTML = "" }
        /*.then(data => document.querySelector("#errorcustom").innerHTML = data);*/
    }
    else {
        alert("Please enter valid url!")
    }

}

function CopyToClipBoard() {
    let shorturlinput = document.querySelector('#shorturl');
    navigator.clipboard.writeText(shorturlinput.value);
    alertclip();
}

function CopyToClipBoardCustom() {
    let shorturlinput = "https://localhost:7207/s/" + document.querySelector('#customshorturl').value;
    navigator.clipboard.writeText(shorturlinput.value);
    alert("Succefuly copy! " + shorturlinput);
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
    openbtn.removeEventListener("click", open)
}

function ClearUrlCustom() {
    document.getElementById("customfullurl").value = "";
    document.getElementById("customshorturl").value = "";
    document.getElementById("customurl").style.display = "none";
    document.getElementById("errorcustom").style.display = "none";
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