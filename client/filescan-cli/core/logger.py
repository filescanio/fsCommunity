import colorama


class Logger:
    """Logger class based on colorama"""

    initialized = False

    def __init__(self):
        if not self.initialized:
            colorama.init(True)
            self.initialized = True


    def error(self, message: str):
        """Log error message"""

        print(colorama.Style.RESET_ALL)
        print(colorama.Fore.RED + message)


    def warning(self, message: str):
        """Log warning message"""

        print(colorama.Fore.YELLOW + message)


    def success(self, message: str):
        """Log success message"""

        print(colorama.Fore.GREEN + message)


    def log(self, color: str, message: str):
        """Log custom color message"""

        print(colorama.Fore.__getattribute__(color) + message)


    def reset(self):
        """Reset all styles"""

        print(colorama.Style.RESET_ALL)

system_logger = Logger()
