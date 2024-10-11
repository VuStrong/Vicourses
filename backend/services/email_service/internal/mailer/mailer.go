package mailer

import (
	"github.com/VuStrong/Vicourses/backend/services/email_service/internal/config"
	"github.com/VuStrong/Vicourses/backend/services/email_service/internal/models"
	"gopkg.in/gomail.v2"
)

type Mailer struct {
	dialer *gomail.Dialer
}

func NewMailer(c *config.Config) *Mailer {
	smtp := c.Smtp
	dialer := gomail.NewDialer(smtp.Host, smtp.Port, smtp.User, smtp.Password)

	return &Mailer{dialer: dialer}
}

func (mailer *Mailer) SendEmail(email *models.EmailModel) error {
	msg := gomail.NewMessage()
	msg.SetHeader("From", email.From)
	msg.SetHeader("To", email.To)
	msg.SetHeader("Subject", email.Subject)
	msg.AddAlternative(email.ContentType, email.Body)

	return mailer.dialer.DialAndSend(msg)
}
