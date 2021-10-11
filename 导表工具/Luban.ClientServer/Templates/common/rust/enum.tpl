{{~if comment != '' ~}}
/**
 * {{comment}}
 */
{{~end~}}
#[allow(dead_code)]
#[allow(non_camel_case_types)]
pub enum {{rust_full_name}} {
    {{~for item in items ~}}
{{~if item.comment != '' ~}}
    /**
     * {{item.comment}}
     */
{{~end~}}
    {{item.name}} = {{item.int_value}},
    {{~end~}}
}

