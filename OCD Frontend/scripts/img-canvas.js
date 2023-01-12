var p5Div = document.getElementById("canvas_selector");
let canvas;
var pg;

var p1 = null;
var p2 = null;

var p1n = null;
var p2n = null;

var change = false;

$(document).ready(() => {
    
});

function setup(){
    //createCanvas(900,900);
    let w = Utils.elementWidth(p5Div)
    canvas = createCanvas(w, 800,P2D);
    canvas.parent(p5Div);
}

function draw(){
    if(change){
        p1 = null;
        p2 = null;
    }

    background(42);

    if((pg)&&(!change)){
        image(pg,0,0,Utils.elementWidth(p5Div),Utils.elementHeight(p5Div));
    }
    
    if(p1){
        if(p2){
            stroke(0,255,0);
            fill(0,0,0,0);

            rect(p1.x,p1.y,p2.x-p1.x,p2.y-p1.y);
        }else{
            stroke(255,0,0);
            fill(0,0,0,0);
            rect(p1.x,p1.y,mouseX-p1.x,mouseY-p1.y);
        }
    }
}

function GetyImg(imageUrl){

    change = true;

    pg = loadImage(imageUrl, ()=>{
        let w = Utils.elementWidth(p5Div);
        
        ratio = pg.height/pg.width;
        
        resizeCanvas(w, w * ratio);
        change = false;
    });

}

function mouseClicked(){

    if((mouseX > 0)&&(mouseX < Utils.elementWidth(p5Div))){
        if((mouseY > 0)&&(mouseY < Utils.elementHeight(p5Div))){
            if(p1 == null){
                p1 = {
                    x:mouseX,
                    y:mouseY
                }

                p1n = {
                    x : mouseX / Utils.elementWidth(p5Div),
                    y : mouseY / Utils.elementHeight(p5Div)
                }

            }else if(p1){
        
                if(p2 == null){
                    p2 = {
                        x:mouseX,
                        y:mouseY
                    }
                    p2n = {
                        x : mouseX / Utils.elementWidth(p5Div),
                        y : mouseY / Utils.elementHeight(p5Div)
                    }
                }else{
                    p1 = null;
                    p2 = null;
                    p1n = null;
                    p2n = null;
                    console.log("reset region");
                }
        
            }
        }else{
            console.log("hors zone",mouseX,mouseY);
        }
    }

    

}