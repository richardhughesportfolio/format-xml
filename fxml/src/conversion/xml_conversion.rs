pub fn convert(input: &str) -> String {
    String::from(input)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn convert_empty_input_returns_empty_result() {
        let input = "";

        let result = convert(input);

        assert_eq!("", result);
    }

    #[test]
    fn convert_invalid_input_returns_input() {
        let input = "invalid XML...";

        let result = convert(input);

        assert_eq!(input, result);
    }
}
