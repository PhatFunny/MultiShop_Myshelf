try{

const slides = document.querySelectorAll(".slide");
const slideContainer = document.querySelector(".slides");

const btnPrevSlide = document.querySelector(".slide-prev");
const btnNextSlide = document.querySelector(".slide-next");

const dots = document.querySelectorAll(".dot");
const dotContainer = document.querySelector(".dots");

let currentSlide = 0;

const maxSlide = slides.length - 1;

const updateActiveDot = function (curSlide) {
    dots.forEach((dot, i) => {
        if (i === curSlide) {
            dot.classList.add("dot-active");
            return;
        }
        dot.classList.remove("dot-active");
    });
}

const goToSlide = function (slide) {
    slides.forEach((el, i) => {
        el.style.transform = `translateX(${100 * (i - slide)}%)`;
    });

    updateActiveDot(slide);
}

goToSlide(currentSlide);

const nextSlide = function () {
    if (currentSlide === maxSlide) {
        currentSlide = 0;
    } else {
        currentSlide++;
    }
    goToSlide(currentSlide);
}

const prevSlide = function () {
    if (currentSlide === 0) {
        currentSlide = maxSlide;
    } else {
        currentSlide--;
    }
    goToSlide(currentSlide);
}

let autoNextSlide = setInterval(() => { nextSlide(); }, 3000); 

btnPrevSlide.addEventListener("click", () => {
    prevSlide();
    clearInterval(autoNextSlide); // Reset thoi gian chuyen slide
    autoNextSlide = setInterval(() => { nextSlide(); }, 3000);
});
btnNextSlide.addEventListener("click", () => {
    nextSlide();
    clearInterval(autoNextSlide);
    autoNextSlide = setInterval(() => { nextSlide(); }, 3000);
});

const initDots = function (dots) {
    dots.forEach((dot, i) => {
        dot.addEventListener("click", function () {
            goToSlide(i);
            clearInterval(autoNextSlide);
            autoNextSlide = setInterval(() => { nextSlide(); }, 3000);
        })
    });
}


initDots(dots);
} catch (err) {
    console.log(err.message);
}

const toTopBtn = document.querySelector(".to-top-btn");


window.addEventListener("scroll", () => {
    if (window.pageYOffset > 100) {
        toTopBtn.classList.add("active");
    } else {
        toTopBtn.classList.remove("active");
    }
});

const header = document.querySelector(".nn-header");
toTopBtn.addEventListener("click", () => {
    header.scrollIntoView({ behavior: "smooth" });
});