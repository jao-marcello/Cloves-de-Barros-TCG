// Lista de habilidades possíveis no jogo (como os Sigilos do Inscryption)
public enum CardAbility
{
    None,               // Sem habilidade
    Airborne,           // Ataque Aéreo (ignora a carta da frente e bate no jogador)
    BifurcatedStrike,   // Ataque Direcional (ataca nas diagonais)
    Spikes,             // Espinhos (causa dano a quem atacar esta carta)
    TouchOfDeath        // Toque da Morte (mata qualquer carta com 1 hit, independente da vida)
}