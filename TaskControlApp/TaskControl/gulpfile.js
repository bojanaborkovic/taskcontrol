// include plug-ins
var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');
var minifyCSS = require('gulp-minify-css');
var copy = require('gulp-copy');
var bower = require('gulp-bower');
var sourcemaps = require('gulp-sourcemaps');                            


//var config = {
//  //Include all js files but exclude any min.js files
//  src: ['Scripts/*.js', '!Scripts/*.min.js', '!Scripts/_references.js']
//}

var config = {
  //JavaScript files that will be combined into a jquery bundle
  jquerysrc: [
      'bower_components/jquery/dist/jquery.min.js',
      'bower_components/jquery-validation/dist/jquery.validate.min.js',
      'bower_components/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js'    
  ],
  jquerybundle: 'Scripts/jquery-bundle.min.js',
                                                          
  jqueryuisrc: ['bower_components/jquery-ui/jquery-ui.js'],
  jqueryuibundle: 'Scripts/jqueryui-bundle.min.js',

  masonrysrc: ['bower_components/masonry/dist/masonry.pkgd.js'],
  masonrybundle : 'Scripts/masonry-bundle.min.js',

  //JavaScript files that will be combined into a Bootstrap bundle
  bootstrapsrc: [
      'bower_components/bootstrap/dist/js/bootstrap.min.js',
      'bower_components/respond/dest/respond.min.js'
  ],
  bootstrapbundle: 'Scripts/bootstrap-bundle.min.js',

  //Modernizr
  modernizrsrc: ['bower_components/modernizr/modernizr.custom.js'],
  modernizrbundle: 'Scripts/modernizer.min.js',

  //Bootstrap CSS and Fonts
  bootstrapcss: 'bower_components/bootstrap/dist/css/bootstrap.css',
  boostrapfonts: 'bower_components/bootstrap/dist/fonts/*.*',
  appcss: 'Content/Site.css',
  fontsout: 'Content/dist/fonts/',
  cssout: 'Content/dist/css/'

  //src: ['Scripts/*.js', '!Scripts/*.min.js', '!Scripts/_references.js']

}

//delete the output file(s)
//gulp.task('clean', function () {
//  //del is an async function and not a gulp plugin (just standard nodejs)
//  //It returns a promise, so make sure you return that from this task function
//  //  so gulp knows when the delete is complete
//  return del(['all.min.js']);
//});

// Synchronously delete the output script file(s)
//gulp.task('clean-vendor-scripts', function (cb) {
//  del([config.jquerybundle,
//            config.bootstrapbundle,
//            config.modernizrbundle], cb);
//});

// Combine and minify all files from the app folder
// This tasks depends on the clean task which means gulp will ensure that the 
// Clean task is completed before running the scripts task.
//gulp.task('scripts', ['clean'], function () {

//  return gulp.src(config.src)
//    .pipe(uglify())
//    .pipe(concat('all.min.js'))
//    .pipe(gulp.dest('Scripts/'));
//});

//Create a jquery bundled file
gulp.task('jquery-bundle', [ 'bower-restore'], function () {
  return gulp.src(config.jquerysrc)
      .pipe(uglify().on('error', function (e) {
        console.log(e);
      })).pipe(concat('jquery-bundle.min.js'))
      .pipe(gulp.dest('Scripts/'));
   //.pipe(concat('jquery-bundle.min.js'))
   //.pipe(gulp.dest('Scripts/'));
});

//Create a jqueryui bundled file
gulp.task('jqueryui-bundle', ['bower-restore'], function () {
  return gulp.src(config.jqueryuisrc)
      .pipe(uglify().on('error', function (e) {
        console.log(e);
      })).pipe(concat('jqueryui-bundle.min.js'))
      .pipe(gulp.dest('Scripts/'));
});

//Create a masonry bundled file
gulp.task('masonry-bundle', ['bower-restore'], function () {
  return gulp.src(config.jqueryuisrc)
      .pipe(uglify().on('error', function (e) {
        console.log(e);
      })).pipe(concat('masonry-bundle.min.js'))
      .pipe(gulp.dest('Scripts/'));
});


//Create a bootstrap bundled file
gulp.task('bootstrap-bundle', ['bower-restore'], function () {
  return gulp.src(config.bootstrapsrc)
   .pipe(sourcemaps.init())
   .pipe(concat('bootstrap-bundle.min.js'))
   .pipe(sourcemaps.write('maps'))
   .pipe(gulp.dest('Scripts/'));
});

//Create a modernizr bundled file
gulp.task('modernizer', ['bower-restore'], function () {
  return gulp.src(config.modernizrsrc)
      .pipe(sourcemaps.init())
      .pipe(uglify())
      .pipe(concat('modernizer-min.js'))
      .pipe(sourcemaps.write('maps'))
      .pipe(gulp.dest('Scripts/'));
});


//gulp.task('scripts', ['clean'], function () {
//  return gulp.src(config.src)
//    .pipe(uglify().on('error', function (e) {
//      console.log(e);
//    })).pipe(concat('all.min.js'))
//    .pipe(gulp.dest('Scripts/'));
//});


// Combine and the vendor files from bower into bundles (output to the Scripts folder)
gulp.task('vendor-scripts', ['jquery-bundle', 'jqueryui-bundle', 'bootstrap-bundle', 'modernizer', 'masonry-bundle'], function () {

});

// Synchronously delete the output style files (css / fonts)
gulp.task('clean-styles', function (cb) {
  del([config.fontsout,
            config.cssout], cb);
});
                                                                            
gulp.task('css', ['bower-restore'], function () {
  return gulp.src([config.bootstrapcss, config.appcss])
   .pipe(concat('app.css'))
   .pipe(gulp.dest(config.cssout))
   .pipe(minifyCSS())                                                       
   .pipe(concat('app.min.css'))
   .pipe(gulp.dest(config.cssout));
});

//gulp.task('fonts', ['bower-restore'], function () {
//  return
//  gulp.src(config.boostrapfonts)
//      .pipe($.filter('*.{eot,svg,ttf,woff,woff2}'))
//      .pipe($.flatten())
//      .pipe(gulp.dest(config.fontsout));
//});

//copy over fonts
gulp.task('fonts', function () {
  return gulp.src('bower_components/bootstrap/dist/fonts/*.{eot,svg,ttf,woff,woff2}')
    //.pipe($.flatten())
    .pipe(gulp.dest(config.fontsout));
});

// Combine and minify css files and output fonts
gulp.task('styles', ['css', 'fonts'], function () {

});

//Restore all bower packages
gulp.task('bower-restore', function () {
  return bower();
});


//Set a default tasks
gulp.task('default', ['vendor-scripts', 'styles'], function () { });

//gulp.task('watch', function () {
//  return gulp.watch(config.src, ['scripts']);
//}); 