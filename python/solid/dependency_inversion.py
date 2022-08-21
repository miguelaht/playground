import abc

class MailerInterface(metaclass=abc.ABCMeta):
    """Send emails"""
    @classmethod
    def __subclasshook__(cls, subclass):
        return (hasattr(subclass, "send") and
                callable(subclass.send))

    @abc.abstractmethod
    def send(self, email: str):
        raise NotImplementedError

class PaymentSenderInterface(metaclass=abc.ABCMeta):
    """Process payment"""
    @classmethod
    def __subclasshook__(cls, subclass):
        return (hasattr(subclass, "send") and
                callable(subclass.send))

    @abc.abstractmethod
    def send(self, payment: str):
        raise NotImplementedError

class LoggerInterface(metaclass=abc.ABCMeta):
    """App logging"""
    @classmethod
    def __subclasshook__(cls, subclass):
        return (hasattr(subclass, "info") and
                callable(subclass.send) and
                hasattr(subclass, "error") and
                callable(subclass.send))

    @abc.abstractmethod
    def info(self, msg: str):
        """Print info log message"""
        raise NotImplementedError

    @abc.abstractmethod
    def error(self, msg: str):
        """Print error log message"""
        raise NotImplementedError

class Mailer(MailerInterface):
    def send(self, _: str):
        print("Sending email")

class PaymentSender(PaymentSenderInterface):
    def send(self, _: str):
        print("Processing payment")

class Logger(LoggerInterface):
    def info(self, _: str):
        print("INFO")

    def error(self, _: str):
        print("ERROR")

class Payment:
    def __init__(self, amount: int, address: str, email: str):
        self.amount = amount
        self.address = address
        self.email = email

class PaymentService:
    def __init__(
        self,
        mailer: MailerInterface,
        payment_sender: PaymentSenderInterface,
        logger: LoggerInterface
    ):
        self.mailer = mailer
        self.payment_sender = payment_sender
        self.logger = logger

    def handle(self, payment: Payment):
        try:
            self.payment_sender.send(payment.address)
        except SystemError: # example error, must be replaced with appropriate error
            self.logger.error("Error while sending payment")
        else:
            self.logger.info("Payment sent successfully")

if __name__ == "__main__":
    mailer = Mailer()
    payment_sender = PaymentSender()
    logger = Logger()

    payment = Payment(100, "Bank of Bikiny Bottom", "xyz@xyz.com")
    payment_service = PaymentService(mailer, payment_sender, logger)
    payment_service.handle(payment)
