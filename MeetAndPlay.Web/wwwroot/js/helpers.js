$('.dropdown-menu option, .dropdown-menu select').click(function(e) {
    e.stopPropagation();
});

function showModal(element){
    $(element).modal("show");
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