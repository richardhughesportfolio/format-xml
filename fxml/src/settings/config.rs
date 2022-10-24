pub struct Config {
    strict: bool,
    help: bool,
}

impl Config {
    pub fn new(args: &Vec<String>) -> Self {
        Self {
            strict: arguments_have_flag(args, "--strict"),
            help: arguments_have_flag(args, "--help"),
        }
    }

    pub fn strict(&self) -> bool {
        self.strict
    }

    pub fn help(&self) -> bool {
        self.help
    }
}

fn arguments_have_flag(args: &Vec<String>, flag: &str) -> bool {
    for arg in args {
        if arg == &flag {
            return true;
        }
    }

    false
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn new_no_arguments_returns_default_config() {
        let args: Vec<String> = Vec::new();

        let result = Config::new(&args);

        assert_eq!(false, result.strict);
        assert_eq!(false, result.help);
    }

    #[test]
    fn new_has_help_argument_returns_valid_config() {
        let args = vec![String::from("--help")];

        let result = Config::new(&args);

        assert_eq!(false, result.strict);
        assert_eq!(true, result.help);
    }

    #[test]
    fn new_has_strict_argument_returns_valid_config() {
        let args = vec![String::from("--help")];

        let result = Config::new(&args);

        assert_eq!(false, result.strict);
        assert_eq!(true, result.help);
    }

    #[test]
    fn strict_returns_valid_result() {
        let result = Config {
            strict: true,
            help: false,
        };

        assert_eq!(true, result.strict());
    }

    #[test]
    fn help_returns_valid_result() {
        let result = Config {
            strict: false,
            help: true,
        };

        assert_eq!(true, result.help());
    }
}
