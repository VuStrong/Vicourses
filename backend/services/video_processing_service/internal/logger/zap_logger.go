package logger

import (
	"os"

	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/config"
	"go.uber.org/zap"
	"go.uber.org/zap/zapcore"
	"gopkg.in/natefinch/lumberjack.v2"
)

type Logger interface {
	Debug(args ...interface{})
	Debugf(template string, args ...interface{})
	Info(args ...interface{})
	Infof(template string, args ...interface{})
	Warn(args ...interface{})
	Warnf(template string, args ...interface{})
	Error(args ...interface{})
	Errorf(template string, args ...interface{})
	DPanic(args ...interface{})
	DPanicf(template string, args ...interface{})
	Fatal(args ...interface{})
	Fatalf(template string, args ...interface{})
}

type appLogger struct {
	cfg         *config.Config
	sugarLogger *zap.SugaredLogger
}

func NewAppLogger(cfg *config.Config) *appLogger {
	return &appLogger{cfg: cfg}
}

var loggerLevelMap = map[string]zapcore.Level{
	"debug":  zapcore.DebugLevel,
	"info":   zapcore.InfoLevel,
	"warn":   zapcore.WarnLevel,
	"error":  zapcore.ErrorLevel,
	"dpanic": zapcore.DPanicLevel,
	"panic":  zapcore.PanicLevel,
	"fatal":  zapcore.FatalLevel,
}

func (l *appLogger) getLoggerLevel() zapcore.Level {
	level, exist := loggerLevelMap[l.cfg.Logger.Level]
	if !exist {
		return zapcore.DebugLevel
	}

	return level
}

func (l *appLogger) InitLogger() {
	logLevel := l.getLoggerLevel()

	encoderCfg := zap.NewProductionEncoderConfig()
	encoderCfg.EncodeTime = zapcore.ISO8601TimeEncoder

	w := zapcore.AddSync(&lumberjack.Logger{
		Filename:   "logs/video-processing-service.log",
		MaxSize:    100, // MB
		MaxBackups: 2,
		MaxAge:     2, // days
	})

	core := zapcore.NewTee(
		zapcore.NewCore(
			zapcore.NewJSONEncoder(encoderCfg),
			w,
			logLevel,
		),
		zapcore.NewCore(
			zapcore.NewConsoleEncoder(encoderCfg),
			zapcore.AddSync(os.Stdout),
			logLevel,
		),
	)

	logger := zap.New(core)

	l.sugarLogger = logger.Sugar()
}

func (l *appLogger) Debug(args ...interface{}) {
	l.sugarLogger.Debug(args...)
}

func (l *appLogger) Debugf(template string, args ...interface{}) {
	l.sugarLogger.Debugf(template, args...)
}

func (l *appLogger) Info(args ...interface{}) {
	l.sugarLogger.Info(args...)
}

func (l *appLogger) Infof(template string, args ...interface{}) {
	l.sugarLogger.Infof(template, args...)
}

func (l *appLogger) Warn(args ...interface{}) {
	l.sugarLogger.Warn(args...)
}

func (l *appLogger) Warnf(template string, args ...interface{}) {
	l.sugarLogger.Warnf(template, args...)
}

func (l *appLogger) Error(args ...interface{}) {
	l.sugarLogger.Error(args...)
}

func (l *appLogger) Errorf(template string, args ...interface{}) {
	l.sugarLogger.Errorf(template, args...)
}

func (l *appLogger) DPanic(args ...interface{}) {
	l.sugarLogger.DPanic(args...)
}

func (l *appLogger) DPanicf(template string, args ...interface{}) {
	l.sugarLogger.DPanicf(template, args...)
}

func (l *appLogger) Panic(args ...interface{}) {
	l.sugarLogger.Panic(args...)
}

func (l *appLogger) Panicf(template string, args ...interface{}) {
	l.sugarLogger.Panicf(template, args...)
}

func (l *appLogger) Fatal(args ...interface{}) {
	l.sugarLogger.Fatal(args...)
}

func (l *appLogger) Fatalf(template string, args ...interface{}) {
	l.sugarLogger.Fatalf(template, args...)
}
