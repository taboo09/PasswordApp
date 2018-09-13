import { PipeTransform, Pipe } from "@angular/core";
import { Password } from "../_models/Password";

@Pipe({
    name: 'passwordsFilter'
})
export class PasswordFilterPipe implements PipeTransform{
    transform(passwords: Password[], searchField: string): Password[] {
        if (!passwords || !searchField || !searchField.replace(/\s/g, '').length) return passwords;

        return passwords.filter(password => password.name.toLowerCase()
            .indexOf(searchField.toLowerCase()) !== -1);
    }
}