(function(){
    window.onload = initSlideshow;

    function getRandomInt(min, max) {
        min = Math.ceil(min);
        max = Math.floor(max);
        return Math.floor(Math.random() * (max - min)) + min; //The maximum is exclusive and the minimum is inclusive
    }
    
    function initSlideshow() {
        let bg = document.getElementById('album-art-bg');
        for (let i = 0; i < 10; ++i) {
            let slider = document.createElement('div');
            slider.classList.add('album-art-slider');
            slider.classList.add(Math.random());
            slider.style.animationDelay = `${Math.random() * 5}s`
            for (let i = 0; i < 25; ++i)
                slider.innerHTML += `<img loading='lazy' src="/album-arts/${getRandomInt(1, 167)}.jpg">`;

            bg.appendChild(slider);
        }
    }
})();