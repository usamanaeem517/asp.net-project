$(document).ready(function () {
    var audioElement = document.createElement('audio');

    $(".play-init").click(function () {

        if ($('#no-play').css('display') != 'none') {
            $('#yes-play').show().siblings('div').hide();
        }
        let metadata = JSON.parse(this.dataset.metadata);
        $("#album-art").attr("src","/Music/" + this.id + ".jpg");
        $("#name").text(metadata.song_name);
        $("#artist").text(metadata.singer_name);
        $("#album").text(metadata.album_name);
        $("#genre").text(metadata.genre);
        $("#vlink").click(function () {
            window.location=metadata.v_link;
        });
        
        audioElement.setAttribute("src", "/Music/" + this.id + ".mp3");
        audioElement.play();

    });

    audioElement.addEventListener('ended', function () {
        this.play();
    }, false);

    $('#play').click(function () {
        audioElement.play();
        $("#status").text("Status: Playing");
    });

    $('#pause').click(function () {
        audioElement.pause();
        $("#status").text("Status: Paused");
    });

    $('#restart').click(function () {
        audioElement.currentTime = 0;
    });


    var timer = 0;
    var percent = 0;
    audioElement.addEventListener('playing', function () {
        var duration = audioElement.duration;
        advance(duration, audioElement);
    });

    audioElement.addEventListener('pause', function () {
        clearTimeout(timer);
    });

    $("#progress").click(function (e) {
        var perc = e.offsetX / parseInt($("#progress").css("width"));
        audioElement.currentTime = perc * audioElement.duration;
    });

    var advance = function (duration, element) {
        var progress = $("#progress");
        var progressBar = $("#progress .progress-bar");
        var percent = element.currentTime / element.duration * parseInt(progress.css("width"));
        progressBar.css('width', percent + 'px');
        startTimer(duration, element);
    }
    var startTimer = function (duration, element) {
        if (percent < 100) {
            timer = setTimeout(function () { advance(duration, element) }, 300);
        }
    }

    
    $(".fav").click(function (e) {
        let metadata = JSON.parse(this.dataset.metadata);
        e.preventDefault();
        
        $.post('/MyFirst/add_to_playlist', { id: metadata.ID }, function (data) {
            alert("Changes may have been made, please refresh the page!");
        });
    });

    $(".cncrt-add").click(function (e) {
        let metadata = JSON.parse(this.dataset.metadata);
        e.preventDefault();
        $.post('/MyFirst/add_to_concert_att', { id: metadata.ID  }, function (data) {
            alert("Changes may have been made, please refresh the page!");
        });
    });

    $(".rate span").click(function () {
        
        let metadata = JSON.parse(this.parentElement.dataset.metadata);
        var rating = this.dataset.rating;
        let self = $(this);
        $.post('/MyFirst/rate_song', { id: metadata.ID, rating: rating }, function (data) {
            self.siblings().removeClass('rated-star');
            self.addClass('rated-star');
        });
    });


    $("#logout").click(function () {
        
        $.post('/MyFirst/logout', {}, function (data) {
        });
    });

});