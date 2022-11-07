use xmlparser;

pub fn convert(input: &str) -> String {
    let mut output = String::new();
    output.reserve(input.len() * 2);

    let mut indent = 0;

    for token in xmlparser::Tokenizer::from(input) {
        match token {
            Err(_) => return String::from(input),
            Ok(t) => match t {
                // xmlparser::Token::Declaration { version, encoding, standalone, span } => todo!(),
                // xmlparser::Token::ProcessingInstruction { target, content, span } => todo!(),
                // xmlparser::Token::Comment { text, span } => todo!(),
                // xmlparser::Token::DtdStart { name, external_id, span } => todo!(),
                // xmlparser::Token::EmptyDtd { name, external_id, span } => todo!(),
                // xmlparser::Token::EntityDeclaration { name, definition, span } => todo!(),
                // xmlparser::Token::DtdEnd { span } => todo!(),
                xmlparser::Token::ElementStart { prefix: _, local: _, span } => {
                    insert_indented_text(indent, &span, &mut output);
                },
                // xmlparser::Token::Attribute { prefix, local, value, span } => todo!(),
                xmlparser::Token::ElementEnd { end, span } => {
                    match end {
                        xmlparser::ElementEnd::Open => {
                            output.push_str(">\n");
                            indent += 1;
                        },
                        xmlparser::ElementEnd::Close(_, _) => {
                            indent -= 1;
                            insert_indented_text(indent, &span, &mut output);
                        },
                        xmlparser::ElementEnd::Empty => {
                            insert_indented_text(indent, &span, &mut output);
                        }
                    }
                },
                xmlparser::Token::Text { text } => {
                    if !text.is_empty() {
                        insert_indented_text(indent, &text, &mut output);
                        output.push('\n');
                    }
                },
                // xmlparser::Token::Cdata { text, span } => todo!(),
                _ => continue,
            },
        };
    }

    output
}

fn insert_indented_text(indent: i32, text: &str, output: &mut String) {
    let indent_size = 4;

    let mut i = 0;
    while i < indent * indent_size {
        output.push(' ');
        i += 1;
    }

    output.push_str(text);
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

    fn valid_convert_test_cases() -> Vec<(&'static str, &'static str)> {
        vec![("<tag/>", "<tag/>"),
            ("<tag />", "<tag/>"),
            ("<tag></tag>", r#"<tag>
</tag>"#),
            ("<tag>text</tag>", r#"<tag>
    text
</tag>"#)]
    }

    #[test]
    fn convert_valid_input_returns_formatted_input() {
        for (input, expected) in valid_convert_test_cases() {
            let result = convert(input);
            assert_eq!(expected, result);
        }
    }
}
