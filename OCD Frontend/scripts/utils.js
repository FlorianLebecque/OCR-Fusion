    //https://stackoverflow.com/questions/71034739/how-to-extend-the-canvas-to-the-size-of-the-parent-div-with-p5js  --- fguillen
class Utils {
    // Calculate the Width in pixels of a Dom element
    static elementWidth(element) {
      return (
        element.clientWidth -
        parseFloat(window.getComputedStyle(element, null).getPropertyValue("padding-left")) -
        parseFloat(window.getComputedStyle(element, null).getPropertyValue("padding-right"))
      )
    }
  
    // Calculate the Height in pixels of a Dom element
    static elementHeight(element) {
      return (
        element.clientHeight -
        parseFloat(window.getComputedStyle(element, null).getPropertyValue("padding-top")) -
        parseFloat(window.getComputedStyle(element, null).getPropertyValue("padding-bottom"))
      )
    }
}