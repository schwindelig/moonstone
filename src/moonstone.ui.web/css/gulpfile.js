// Include gulp
var gulp = require('gulp');

gulp.task('autoprefixer', function () {
    var postcss      = require('gulp-postcss');
    var sourcemaps   = require('gulp-sourcemaps');
    var autoprefixer = require('autoprefixer');

    return gulp.src('./src/*.css')
        .pipe(postcss([ autoprefixer({ browsers: ['last 2 versions'] }) ]))
        .pipe(gulp.dest('./dist'));
});

// Default Task
gulp.task('default', ['autoprefixer', 'watch']);

// Watch Files For Changes
gulp.task('watch', function() {
    gulp.watch('src/*.css', ['autoprefixer']);
});