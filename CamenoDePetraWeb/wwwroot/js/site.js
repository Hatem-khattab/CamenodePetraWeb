/*******************************
 * REVIEWS CAROUSEL (Optimized)
 *******************************/
let currentSlide = 0;
let autoPlayInterval;
const autoPlaySpeed = 4000;
const reviewsCarousel = document.getElementById('reviewsCarousel');
const dotsContainer = document.getElementById('carouselDots');
const reviewCards = Array.from(document.querySelectorAll('.review-card'));
const container = document.querySelector('.reviews-carousel-container');
if (container) {
    container.addEventListener('mouseenter', () => clearInterval(autoPlayInterval));
    container.addEventListener('mouseleave', startAutoPlay);
}

function getCardsPerSlide() {
    if (window.innerWidth <= 576) return 1;
    if (window.innerWidth <= 768) return 2;
    return 3;
}

let cardsPerSlide = getCardsPerSlide();
let totalSlides = Math.ceil(reviewCards.length / cardsPerSlide);

function initCarousel() {
    cardsPerSlide = getCardsPerSlide();
    totalSlides = Math.ceil(reviewCards.length / cardsPerSlide);
    currentSlide = 0;
    createDots();
    updateCarousel();
    startAutoPlay();
}

function createDots() {
    dotsContainer.innerHTML = '';
    for (let i = 0; i < totalSlides; i++) {
        const dot = document.createElement('div');
        dot.className = `dot ${i === 0 ? 'active' : ''}`;
        dot.addEventListener('click', () => goToSlide(i));
        dotsContainer.appendChild(dot);
    }
}

function updateCarousel() {
    const cardWidth = reviewCards[0].offsetWidth;
    const gap = 30;
    const slideWidth = (cardWidth + gap) * cardsPerSlide;

    // use transform3d for better performance
    reviewsCarousel.style.transform = `translate3d(-${currentSlide * slideWidth}px,0,0)`;

    document.querySelectorAll('.dot').forEach((dot, index) => {
        dot.classList.toggle('active', index === currentSlide);
    });
}

function moveCarousel(direction) {
    currentSlide = (currentSlide + direction + totalSlides) % totalSlides;
    updateCarousel();
    resetAutoPlay();
}

function goToSlide(index) {
    currentSlide = index;
    updateCarousel();
    resetAutoPlay();
}

function startAutoPlay() {
    clearInterval(autoPlayInterval);
    autoPlayInterval = setInterval(() => moveCarousel(1), autoPlaySpeed);
}

function resetAutoPlay() {
    clearInterval(autoPlayInterval);
    startAutoPlay();
}

container.addEventListener('mouseenter', () => clearInterval(autoPlayInterval));
container.addEventListener('mouseleave', startAutoPlay);

window.addEventListener('resize', () => {
    cardsPerSlide = getCardsPerSlide();
    totalSlides = Math.ceil(reviewCards.length / cardsPerSlide);
    if (currentSlide >= totalSlides) currentSlide = totalSlides - 1;
    updateCarousel();
});

/*******************************
 * STARS RATING (Optimized)
 *******************************/
document.addEventListener('DOMContentLoaded', () => {
    const stars = document.querySelectorAll(".stars .star");
    const ratingInput = document.getElementById("ratingValue");

    function highlightStars(rating) {
        stars.forEach((star, i) => {
            star.style.color = i < rating ? "#ffc107" : "#ddd";
        });
    }

    stars.forEach((star, index) => {
        star.addEventListener("mouseenter", () => highlightStars(index + 1));
        star.addEventListener("click", () => {
            ratingInput.value = index + 1;
            highlightStars(index + 1);
        });
    });

    document.querySelector(".stars").addEventListener("mouseleave", () => {
        highlightStars(parseInt(ratingInput.value) || 0);
    });
});

/*******************************
 * ABOUT TEXT ROTATION (Optimized)
 *******************************/
document.addEventListener('DOMContentLoaded', () => {
    const texts = [
        "With over 15 years of experience in organizing tourist programs and guiding travelers",
        "We offer tourism programs across Jordan and various destinations, including flight and hotel bookings",
        "We organize tours, conferences, and adventure trips like hiking and safari tours in Wadi Rum",
        "Every trip is tailored to the interests and preferences of our travelers"
    ];

    const aboutDiv = document.querySelector('.about-us');
    aboutDiv.innerHTML = "<p></p>";
    const p = aboutDiv.querySelector('p');
    p.style.transition = "opacity 0.5s";
    let index = 0;

    function rotateText() {
        p.style.opacity = 0;
        requestAnimationFrame(() => {
            setTimeout(() => {
                p.textContent = texts[index];
                p.style.opacity = 1;
                index = (index + 1) % texts.length;
            }, 500);
        });
    }

    rotateText();
    setInterval(rotateText, 6000);
});

/*******************************
 * IMAGE CAROUSEL (Infinite Loop Fix)
 *******************************/
document.addEventListener('DOMContentLoaded', () => {
    const imgCarousel = document.querySelector('.image-carousel');
    const imgs = Array.from(imgCarousel.children);
    if (imgs.length === 0) return;

    // 1. Clone the images and append them to the end to create a loop effect
    imgs.forEach(img => {
        const clone = img.cloneNode(true);
        imgCarousel.appendChild(clone);
    });

    let imgIndex = 0;
    const totalOriginalImages = imgs.length;
    const imgWidth = imgs[0].offsetWidth + 20; // Ensure this matches your CSS margin/gap

    function moveImages() {
        imgIndex++;
        
        imgCarousel.style.transition = 'transform 0.5s ease';
        imgCarousel.style.transform = `translate3d(-${imgIndex * imgWidth}px, 0, 0)`;

        // 2. When we reach the start of the cloned set, jump back to the beginning instantly
        if (imgIndex >= totalOriginalImages) {
            setTimeout(() => {
                imgCarousel.style.transition = 'none'; // Remove animation for the jump
                imgIndex = 0;
                imgCarousel.style.transform = `translate3d(0, 0, 0)`;
            }, 50); // This delay must match your transition time (0.5s)
        }
    }

    setInterval(moveImages, 3000);
});