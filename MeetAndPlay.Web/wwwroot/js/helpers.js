function showModal(element){
    const modal = new BSN.Modal(element);
    modal.show();
}

function addClass(element, className) {
    console.log(element);
    element.classList.add(className);
}

function removeClass(element, className) {
    element.classList.remove(className);
}

function toggleClass(element, className) {
    element.classList.toggle(className);
}

function containsClass(element, className) {
    return element.classList.contains(className);
}